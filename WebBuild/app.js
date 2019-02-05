var express = require('express');
var path = require('path');
var cookieParser = require('cookie-parser');
var logger = require('morgan');

var indexRouter = require('./routes/index');
var metamaskRouter = require('./routes/metamask');
var noAccountRouter = require('./routes/noaccount');
var gameRouter = require('./routes/game');
var usersRouter = require('./routes/users');
var healthcheckRouter = require('./routes/healthcheck');

var app = express();

app.use(logger('dev'));
app.use(express.json());
app.use(express.urlencoded({ extended: false }));
app.use(cookieParser());
app.use(express.static(path.join(__dirname, 'public')));

app.use('/', indexRouter);
app.use('/metamask', metamaskRouter);
app.use('/noaccount', noAccountRouter);
app.use('/validate', gameRouter);
app.use('/users', usersRouter);
app.use('/healthcheck', healthcheckRouter);

module.exports = app;
