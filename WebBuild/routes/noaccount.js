var express = require('express');
var fs = require('fs');
var router = express.Router();

router.use('/', express.static(__dirname + '/../public/noaccount.html'))

module.exports = router;