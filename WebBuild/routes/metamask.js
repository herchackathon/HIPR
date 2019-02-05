var express = require('express');
var fs = require('fs');
var router = express.Router();

router.use('/', express.static(__dirname + '/../public/metamask.html'))

module.exports = router;