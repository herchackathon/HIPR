var express = require('express');
var path = require('path');
var cookieParser = require('cookie-parser');
var logger = require('morgan');
const port = process.env.PORT || 8000;

var indexRouter = require('./routes/index');
var gameRouter = require('./routes/game');
var usersRouter = require('./routes/users');

var app = express();

app.use(logger('dev'));
app.use(express.json());
app.use(express.urlencoded({ extended: false }));
app.use(cookieParser());
app.use(express.static(path.join(__dirname, 'public')));

app.use('/', indexRouter);
app.use('/validate', gameRouter);
app.use('/users', usersRouter);

app.listen(port, function(){
  console.log('listening on port ' + port)
});

module.exports = app;
