var express = require('express');
var fs = require('fs');
var router = express.Router();

/* GET game page. */
/*router.get('/', function(req, res, next) {
    res.sendFile('game.html', { root: __dirname + '/../public/' });
});
*/

//router.use('/', express.static(path.resolve(__dirname + '/../public/game.html')))
router.use('/', express.static(__dirname + '/../public/game.html'))

module.exports = router;