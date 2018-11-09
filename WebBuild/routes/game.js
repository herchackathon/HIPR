var express = require('express');
var fs = require('fs');
var router = express.Router();

/* GET game page. */
router.get('/', function(req, res, next) {
    res.sendFile('game.html', { root: __dirname + '/../public/' });
});

module.exports = router;