﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MHLab.UI.Tween;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MHLab.UI
{ 
    [RequireComponent(typeof(RectTransform), typeof(Mask), typeof(Image))]
    public class ScreenSwipe : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
    {
        public RectTransform rectTransform { get { return (RectTransform)transform; } }

        public enum SwipeType { Horizonal, Vertical }

        [Header("Swipe")]
        [SerializeField]
        private SwipeType swipeType = SwipeType.Horizonal;

        [SerializeField, Tooltip("Time a swipe must happen within (s)")]
        private float swipeTime = 0.5f;
        private float startTime;
        private bool isSwipe;

        //[SerializeField]
        private Vector2 velocity;

        [SerializeField, Tooltip("Velocity required to change screen")]
        private int swipeVelocityThreshold = 50;

        [SerializeField, Tooltip("Set to true if you want to be able to skip screens in one swipe. (Not fully tested)")]
        private bool skipScreen;

        [SerializeField, Tooltip("Velocity require to skip a screen is skipScreen = true")]
        private int skipScreenVelocityThreshold = 250;

        [Header("Content")]
        [SerializeField, Tooltip("Will contents be masked?")]
        private bool maskContent = true;

        private Mask _mask;
        private Mask Mask
        {
            get
            {
                if (_mask == null)
                    _mask = GetComponent<Mask>();
                return _mask;
            }
        }

        private Image _maskImage;
        private Image MaskImage
        {
            get
            {
                if (_maskImage == null)
                    _maskImage = GetComponent<Image>(); ;
                return _maskImage;
            }
        }

        [SerializeField, Tooltip("Starting screen. Note: a 0 indexed array")]
        private int startingScreen;

        [SerializeField]
        private int currentScreen;
        public int CurrentScreen { get { return currentScreen; } }

        [SerializeField, Tooltip("Parent object which contain the screens")]
        private RectTransform content;
        public RectTransform Content { get { return content; } set { content = value; } }

        [SerializeField, Tooltip("Distance between screens")]
        private float spacing = 20;
        public float Spacing
        {
            get { return spacing; }
            set
            {
                spacing = value;
                SetScreenPositionsAndContentWidth();
            }
        }

        [SerializeField]
        private List<RectTransform> screens;
        public int ScreenCount { get { return screens.Count; } }


        // screen orienation change events
        [Tooltip("Will poll for changes in screen orientation changes. (Mobile)")]
        public bool pollForScreenOrientationChange;
        [SerializeField, Tooltip("A key for testing orientation change event in the editor")]
        private KeyCode editorRefreshKey = KeyCode.F1;
        private ScreenOrientation screenOrientation;


        [SerializeField, Tooltip("Toggle Group to display pagination. (Optional)")]
        private ToggleGroup pagination;
        private Toggle _toggleMockPrfab;
        private List<Toggle> toggles;

        [Header("Controlls (Optional)")]
        [Tooltip("True = Acts like a normal screenRect but with snapping\nFalse = Can only change screens with buttons or from another script")]
        public bool isInteractable = true;

        [SerializeField]
        private Button nextButton;
        public Button NextButton { get { return nextButton; } }
        [SerializeField]
        private Button previousButton;
        public Button PreviousButton { get { return previousButton; } }

        [SerializeField, Tooltip("Previous button disables when current screen is at 0. Next button disables when current screen is at screen count")]
        private bool disableButtonsAtEnds;

        [Header("Tween")]
        [SerializeField, Tooltip("Length of the tween (s)")]
        private float tweenTime = 0.5f;
        [SerializeField]
        private iTween.EaseType easeType = iTween.EaseType.easeOutExpo;

        // bounds
        private Bounds contentBounds;
        private Bounds viewBounds;

        // start positions
        private Vector2 pointerStartLocalCursor;
        private Vector2 dragStartPos;

        // events
        [Serializable]
        public class ScreenEvent : UnityEvent<int> { }

        [Space]
        public UnityEvent onScreenDragBegin;
        public ScreenEvent onScreenChanged;
        public ScreenEvent onScreenTweenEnd;

        private void Start()
        {
            SetScreenPositionsAndContentWidth();
            Pagination_Init();
            GoToScreen(startingScreen);

            // button listeners
            if (previousButton)
                previousButton.onClick.AddListener(GoToPreviousScreen);

            if (nextButton)
                nextButton.onClick.AddListener(GoToNextScreen);
        }

        private IEnumerator CheckForOrientationChange()
        {
            // set initial orientation
            screenOrientation = Screen.orientation;

            while (enabled)
            {
                if (screenOrientation != Screen.orientation || (Application.isEditor && Input.GetKeyDown(editorRefreshKey)))
                {
                    screenOrientation = Screen.orientation;

                    Debug.LogFormat("SwcreenSwipe Orientation change: {0}", screenOrientation);

                    // refresh contents on the change
                    RefreshContents();
                }
                yield return null;
            }
        }

        private void OnEnable()
        {
            // refresh contents on screen change
            if (pollForScreenOrientationChange)
                StartCoroutine(CheckForOrientationChange());
        }

        private void OnValidate()
        {
            Mask.showMaskGraphic = false;
            Mask.enabled = maskContent;
            MaskImage.enabled = maskContent;
        }

        private void Reset()
        {
            maskContent = true;

            content = transform.GetChild(0) as RectTransform;

            swipeTime = 0.5f;
            swipeVelocityThreshold = 50;
            skipScreenVelocityThreshold = 250;
            easeType = iTween.EaseType.easeOutExpo;
            spacing = 20;
        }

        #region Pagination
        /// <summary>
        /// Initializes the pagination toggles
        /// </summary>
        private void Pagination_Init()
        {
            if (pagination)
            {
                toggles = pagination.GetComponentsInChildren<Toggle>().ToList();

                // store the first toggle to use for instantiating later
                if (toggles[0] != null && _toggleMockPrfab == null)
                    _toggleMockPrfab = toggles[0];

                // loop through and assign toggle properties
                for (int i = 0; i < toggles.Count; i++)
                {
                    toggles[i].isOn = false;
                    toggles[i].group = pagination;
                    toggles[i].onValueChanged.AddListener(PaginationToggleCallback);
                }
            }
        }

        private void SelectToggle(int index)
        {
            if (pagination)
            {
                try
                {
                    toggles[currentScreen].isOn = true;
                }
                catch (ArgumentOutOfRangeException e)
                {
                    Debug.LogError(e);
                    Debug.LogError(index);
                }
            }
        }

        private void AddPaginationToggle()
        {
            if (pagination)
            {
                var newToggle = Instantiate(_toggleMockPrfab, pagination.transform);
                newToggle.group = pagination;
                newToggle.isOn = false;

                // for some reason shit gets turned off so this is a just in case thing
                newToggle.gameObject.SetActive(true);
                newToggle.enabled = true;
                newToggle.GetComponentInChildren<Image>().enabled = true;


                toggles.Add(newToggle);
            }
        }

        private void RemovePaginationToggle(int index)
        {
            // pagination
            if (pagination)
            {
                // remove from list
                toggles.RemoveAt(index);

                // destroy gameobject
                Destroy(toggles[index].gameObject);
            }
        }

        private void RemoveAllPaginationToggles()
        {
            if (pagination)
            {
                // clear toggle list
                toggles.Clear();

                for (int i = 0; i < pagination.transform.childCount; i++)
                {
                    // destroy gameobject
                    Destroy(pagination.transform.GetChild(i).gameObject);
                }
            }
        }

        /// <summary>
        /// Callback function from pagination toggles to change screen opon clicking toggle
        /// </summary>
        /// <param name="ison"></param>
        private void PaginationToggleCallback(bool ison)
        {
            if (ison)
            {
                for (int i = 0; i < toggles.Count; i++)
                {
                    if (toggles[i].isOn)
                    {
                        GoToScreen(i);
                        break;
                    }
                }
            }
        }
        #endregion

        #region Screen Mangement
        /// <summary>
        /// Sets the screens positions and calculates the contents size
        /// </summary>
        private void SetScreenPositionsAndContentWidth()
        {
            Vector2 screenSize = rectTransform.rect.size;

            screens = new List<RectTransform>();

            if (content)
            {
                for (int i = 0; i < content.childCount; i++)
                {
                    // assign to list
                    screens.Add(content.GetChild(i).transform as RectTransform);

                    // pivot and anchors
                    screens[i].pivot = screens[i].anchorMin = screens[i].anchorMax =
                                        swipeType == SwipeType.Horizonal
                                        ? new Vector2(0, 0.5f)
                                        : new Vector2(0.5f, 0);

                    // size
                    screens[i].sizeDelta = screenSize;

                    // scale
                    screens[i].localScale = Vector3.one;

                    // position
                    screens[i].anchoredPosition = swipeType == SwipeType.Horizonal
                                ? new Vector2((screenSize.x * i) + (spacing * i), 0)
                                : new Vector2(0, (screenSize.y * i) + (spacing * i));
                }

                // set content anchords and pivot
                content.pivot = content.anchorMin = content.anchorMax =
                                swipeType == SwipeType.Horizonal
                                ? new Vector2(0, 0.5f)
                                : new Vector2(0.5f, 0);

                // set content size
                content.sizeDelta = swipeType == SwipeType.Horizonal
                                ? new Vector2((screenSize.x + spacing) * screens.Count - spacing, screenSize.y)
                                : new Vector2(screenSize.x, (screenSize.y + spacing) * screens.Count - spacing);
            }
        }

        /// <summary>
        /// Calls private corouitine RefreshContentsCoroutine()
        /// <para>Waits until end of frame and then resets screens and pagination</para>
        /// </summary>
        public void RefreshContents()
        {
            StartCoroutine(RefreshContentsCoroutine());
        }

        /// <summary>
        /// Waits until end of frame and then resets screens and pagination
        /// </summary>
        /// <returns></returns>
        private IEnumerator RefreshContentsCoroutine()
        {
            yield return new WaitForEndOfFrame();
            SetScreenPositionsAndContentWidth();
            Pagination_Init();
        }

        /// <summary>
        /// Adds a screen to the list then recalculates the contents width
        /// </summary>
        /// <param name="newScreen"></param>
        /// <param name="screenNumber">Default as last screen. index 0 - </param>
        public void AddScreen(RectTransform newScreen, int screenNumber = -1)
        {
            newScreen.transform.SetParent(content);

            if (IsWithinScreenCount(screenNumber))
                newScreen.SetSiblingIndex(screenNumber);
            else
                newScreen.SetAsLastSibling();

            // add to list
            screens.Add(newScreen);

            // pagination
            AddPaginationToggle();

            //refresh
            StartCoroutine(RefreshContentsCoroutine());
        }

        /// <summary>
        /// Removes screen from list and recalculates contents width 
        /// </summary>
        /// <param name="screenNumber"></param>
        public void RemoveScreen(int screenNumber, Action callback = null)
        {
            if (IsWithinScreenCount(screenNumber))
            {
                // remove from list
                screens.RemoveAt(screenNumber);

                // destroy gameobject
                Destroy(content.GetChild(screenNumber).gameObject);

                // pagination
                RemovePaginationToggle(screenNumber);

                // refresh
                StartCoroutine(RefreshContentsCoroutine());
            }
            else
                Debug.LogWarningFormat("ScreenNumber: '{0}' doesnt exist", screenNumber);
        }

        public void RemoveAllScreens()
        {
            Debug.Log("Removing Screens : " + content.childCount);

            // clear list
            screens.Clear();

            for (int i = 0; i < content.childCount; i++)
            {
                // destroy screen game object
                Destroy(content.GetChild(i).gameObject);
            }

            RemoveAllPaginationToggles();
        }

        /// <summary>
        /// Tweens to a specific screen
        /// </summary>
        /// <param name="screenNumber"></param>
        public void GoToScreen(int screenNumber)
        {
            if (IsWithinScreenCount(screenNumber))
            {
                // set current screen
                currentScreen = screenNumber;

                // pagination
                SelectToggle(screenNumber);

                // tween screen
                TweenPage(-screens[currentScreen].anchoredPosition);

                // disable buttons if ends are reached
                if (disableButtonsAtEnds && previousButton != null && nextButton != null)
                {
                    if (currentScreen == 0)
                    {
                        previousButton.gameObject.SetActive(false);
                        nextButton.gameObject.SetActive(true);
                    }
                    else if (currentScreen == ScreenCount - 1)
                    {
                        nextButton.gameObject.SetActive(false);
                        previousButton.gameObject.SetActive(true);
                    }
                    else
                    {
                        previousButton.gameObject.SetActive(true);
                        nextButton.gameObject.SetActive(true);
                    }
                }
            }
            else
                Debug.LogErrorFormat("Invalid screen number '{0}'. Must be between 0 and {1}", screenNumber, screens.Count - 1);
        }

        public void GoToNextScreen()
        {
            if (IsWithinScreenCount(CurrentScreen + 1))
                GoToScreen(CurrentScreen + 1);
        }

        public void GoToPreviousScreen()
        {
            if (IsWithinScreenCount(CurrentScreen - 1))
                GoToScreen(CurrentScreen - 1);
        }

        private bool IsWithinScreenCount(int index)
        {
            return index >= 0 && index < screens.Count;
        }
        #endregion

        #region Swipe and Drag Controlls
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left || !isInteractable)
                return;

            if (onScreenDragBegin != null)
                onScreenDragBegin.Invoke();

            // cancel tweening
            iTween.Stop(gameObject);

            // get start data
            dragStartPos = eventData.position;
            startTime = Time.time;

            pointerStartLocalCursor = Vector2.zero;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(content, eventData.position, eventData.pressEventCamera, out pointerStartLocalCursor);
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left || !isInteractable)
                return;

            /// Wrapper fucntion <see cref="ScrollRect.OnDrag(PointerEventData)"/>
            DragContent(eventData);

            // validate swipe boolean
            isSwipe = SwipeValidator(eventData.position);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left || !isInteractable)
                return;

            // validate screen change and sets current screen 
            ScreenChangeValidate();

            // got to screen
            GoToScreen(currentScreen);
        }

        /// <summary>
        /// Validates whether or not a swipe was make.
        /// Reasons for failure is swipe timer expired, or threshold is not met
        /// </summary>
        /// <param name="currentPosition"></param>
        /// <returns></returns>
        private bool SwipeValidator(Vector2 currentPosition)
        {
            // get velocity
            velocity = currentPosition - dragStartPos;

            // is within time
            bool isWithinTime = Time.time - startTime < swipeTime;

            // set to true if it is the swipe type we wanted
            bool isSwipeTypeAndEnoughVelocity;

            // get absolute values of both velocity axis
            var velX = Mathf.Abs(velocity.x);
            var velY = Mathf.Abs(velocity.y);

            if (swipeType == SwipeType.Horizonal)
                isSwipeTypeAndEnoughVelocity = velX > velY && velX > swipeVelocityThreshold;
            else
                isSwipeTypeAndEnoughVelocity = velY > velX && velY > swipeVelocityThreshold;

            // return true if both are true
            return isWithinTime && isSwipeTypeAndEnoughVelocity;
        }

        /// <summary>
        /// Validates whether or not a screen can be changed.
        /// Reasons for failure is we're at the end of the screens list
        /// </summary>
        private void ScreenChangeValidate()
        {
            if (isSwipe)
            {
                // get absolute values of both velocity axis
                var velX = Mathf.Abs(velocity.x);
                var velY = Mathf.Abs(velocity.y);

                int newPageNo = -1;

                if (swipeType == SwipeType.Horizonal)
                {
                    // get direction of swipe
                    var leftSwipe = velocity.x < 0;

                    // check if skip is possible
                    var skip = CanSkipScreen(velX, leftSwipe);

                    // get size of screen jump
                    var screenJump = skip ? 2 : 1;

                    // assign new page number
                    newPageNo = leftSwipe ? currentScreen + screenJump : currentScreen - screenJump;
                }
                else
                {
                    // get direction of swipe
                    var upSwipe = velocity.y < 0;

                    // check if skip is possible
                    var skip = CanSkipScreen(velY, upSwipe);

                    // get size of screen jump
                    var screenJump = skip ? 2 : 1;

                    // assign new page number
                    newPageNo = upSwipe ? currentScreen + screenJump : currentScreen - screenJump;
                }

                // if valid pageNo then update current page and invoke event
                if (IsWithinScreenCount(newPageNo))
                {
                    // change current page
                    currentScreen = newPageNo;

                    // invoke changed event
                    if (onScreenChanged != null)
                        onScreenChanged.Invoke(currentScreen);
                }
            }
        }

        /// <summary>
        /// Returns if it is possible to skip a page
        /// </summary>
        /// <param name="increase"></param>
        /// <returns></returns>
        private bool CanSkipScreen(float velocity, bool increase)
        {
            if (!skipScreen || skipScreenVelocityThreshold <= 0)
                return false;

            return increase ? CurrentScreen < ScreenCount - 2 : CurrentScreen > 1;
        }

        /// <summary>
        /// Tweens the contents position
        /// </summary>
        /// <param name="toPos"></param>
        private void TweenPage(Vector2 toPos)
        {
            iTween.ValueTo(gameObject, iTween.Hash(
                "from", content.anchoredPosition,
                "to", toPos,
                "easeType", easeType,
                "time", tweenTime,
                "onupdate", (Action<Vector2>)(x => SetContentAnchoredPosition(x)),
                "oncomplete", (Action<Vector2>)(_ =>
                {
                    if (onScreenTweenEnd != null)
                        onScreenTweenEnd.Invoke(currentScreen);
                })
                ));
        }
        #endregion

        #region Functions From Unity ScrollRect

        /* Note:
            * Everything in this region I sourced from Unity's ScrollRect script and rejigged to work in this script
            * Sources from: https://bitbucket.org/Unity-Technologies/ui 
            * Folder path: UI/UnityEngine.UI/UI/Core/ScrollRect.cs
            */


        /// <summary>
        /// Wrapper fucntion <see cref="ScrollRect.OnDrag(PointerEventData)"/>
        /// </summary>
        /// <param name="eventData"></param>
        private void DragContent(PointerEventData eventData)
        {
            Vector2 localCursor;
            if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(content, eventData.position, eventData.pressEventCamera, out localCursor))
                return;

            UpdateBounds();

            var pointerDelta = localCursor - pointerStartLocalCursor;
            Vector2 position = content.anchoredPosition + pointerDelta;

            // Offset to get content into place in the view.
            Vector2 offset = CalculateOffset(position - content.anchoredPosition);
            position += offset;

            if (offset.x != 0)
                position.x = position.x - RubberDelta(offset.x, viewBounds.size.x);
            if (offset.y != 0)
                position.y = position.y - RubberDelta(offset.y, viewBounds.size.y);


            SetContentAnchoredPosition(position);
        }

        private void SetContentAnchoredPosition(Vector2 position)
        {
            if (swipeType == SwipeType.Vertical)
                position.x = content.anchoredPosition.x;

            if (swipeType == SwipeType.Horizonal)
                position.y = content.anchoredPosition.y;

            if (position != content.anchoredPosition)
            {
                content.anchoredPosition = position;
                UpdateBounds();
            }
        }

        private void UpdateBounds()
        {
            viewBounds = new Bounds(rectTransform.rect.center, rectTransform.rect.size);
            contentBounds = GetBounds();

            // Make sure content bounds are at least as large as view by adding padding if not.
            // One might think at first that if the content is smaller than the view, scrolling should be allowed.
            // However, that's not how scroll views normally work.
            // Scrolling is *only* possible when content is *larger* than view.
            // We use the pivot of the content rect to decide in which directions the content bounds should be expanded.
            // E.g. if pivot is at top, bounds are expanded downwards.
            // This also works nicely when ContentSizeFitter is used on the content.
            Vector3 contentSize = contentBounds.size;
            Vector3 contentPos = contentBounds.center;
            Vector3 excess = viewBounds.size - contentSize;
            if (excess.x > 0)
            {
                contentPos.x -= excess.x * (content.pivot.x - 0.5f);
                contentSize.x = viewBounds.size.x;
            }

            contentBounds.size = contentSize;
            contentBounds.center = contentPos;
        }

        private Bounds GetBounds()
        {
            var vMin = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            var vMax = new Vector3(float.MinValue, float.MinValue, float.MinValue);

            var toLocal = rectTransform.worldToLocalMatrix;

            Vector3[] m_Corners = new Vector3[4];
            content.GetWorldCorners(m_Corners);
            for (int j = 0; j < 4; j++)
            {
                Vector3 v = toLocal.MultiplyPoint3x4(m_Corners[j]);
                vMin = Vector3.Min(v, vMin);
                vMax = Vector3.Max(v, vMax);
            }

            var bounds = new Bounds(vMin, Vector3.zero);
            bounds.Encapsulate(vMax);
            return bounds;
        }

        private Vector2 CalculateOffset(Vector2 delta)
        {
            Vector2 offset = Vector2.zero;

            Vector2 min = contentBounds.min;
            Vector2 max = contentBounds.max;

            min.x += delta.x;
            max.x += delta.x;
            if (min.x > viewBounds.min.x)
                offset.x = viewBounds.min.x - min.x;
            else if (max.x < viewBounds.max.x)
                offset.x = viewBounds.max.x - max.x;


            return offset;
        }

        private float RubberDelta(float overStretching, float viewSize)
        {
            return (1 - (1 / ((Mathf.Abs(overStretching) * 0.55f / viewSize) + 1))) * viewSize * Mathf.Sign(overStretching);
        }
        #endregion

        #region Editing
        [Header("Editing")]
        [SerializeField]
        [Tooltip("Screen you want to be showing in Game view. Note: 0 indexed array")]
        private int editingScreen;
        public void EditingScreen()
        {
            SetScreenPositionsAndContentWidth();

            if (editingScreen >= 0 && editingScreen < screens.Count)
            {
                content.anchoredPosition = -screens[editingScreen].anchoredPosition;
                Debug.LogFormat("Editing: GoToScreen {0}", editingScreen);
            }
            else
                Debug.LogErrorFormat("Invalid editingScreen value. '{0}'", editingScreen);
        }
        #endregion
    }
}