// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your Javascript code.

/**
 * This method ensures to format a time of a date object using two digits. 
 * 
 * @param {Date} d - Date object like new Date()
 * @returns {String} time string hh:mm:ss
 */
function _timeFormat(d) {
    hours = formatTwoDigits(d.getHours());
    minutes = formatTwoDigits(d.getMinutes());
    seconds = formatTwoDigits(d.getSeconds());
    ms = d.getMilliseconds();
    return hours + ":" + minutes + ":" + seconds + '.' + formatMS(ms);
}

/**
 * This method helps to format a string of two digits.
 * In case the given number is smaller than 10, it will add a leading zero, 
 * e.g. 08 instead of 8
 *
 * @param {Number} n - a number with one or two digits
 * @returns {String} String with two digits
 */
function formatTwoDigits(n) {
    return n < 10 ? '0' + n : n;
}

function formatMS(ms) {
    while ((ms + '').length < 4)
        ms = '0' + ms;
    return ms
}

// ***** SHARED LIBRARY *****
// 
function _getTime() {
    var time = new Date();
    var result =
        ('0' + time.getHours()).slice(-2) + ':' +
        ('0' + time.getMinutes()).slice(-2) + ':' +
        ('0' + time.getSeconds()).slice(-2);
    return result;
}

// Show json idented
function _logObject(object) {
    console.log('\nJSON OBJECT:\n' + JSON.stringify(object, null, 2))
}

// shortcut for this...
function _log(message) {
    console.log('\n' + message);
}

// TERMINAL PROJECT
//
// usage
//=> 3..padLeft() => '03'
//=> 3..padLeft(100,'-') => '--3' 
Number.prototype.padLeft = function (base, chr) {
    var len = (String(base || 10).length - String(this).length) + 1;
    return len > 0 ? new Array(len).join(chr || '0') + this : this;
}

function _formatDateTime(d) {
    return _formatDate(d) + ' ' + _formatTime(d);
}

function _formatDate(d) {
    return [d.getFullYear(), (d.getMonth() + 1).padLeft(), d.getDate().padLeft()].join('-');
}

function _formatTime(d) {
    // HH:MM:SS GMT-0500 (hora estándar de Colombia)
    return d.toTimeString().split(' ')[0];
    // other option:
    // return [d.getHours().padLeft(), d.getMinutes().padLeft(), d.getSeconds().padLeft()].join(':');
}

function _getLocalTime() {
    var time = new Date();
    var result =
        ('0' + time.getHours()).slice(-2) + ':' +
        ('0' + time.getMinutes()).slice(-2) + ':' +
        ('0' + time.getSeconds()).slice(-2);
    return result;
}

function _newGuid() {
    return 'xxxxxxxx-xxxx-4xxx-yxxx-xxxxxxxxxxxx'.replace(/[xy]/g, function (c) {
        var r = Math.random() * 16 | 0, v = c == 'x' ? r : (r & 0x3 | 0x8);
        return v.toString(16);
    });
}

function _getSiteName() {
    return window.location.href.replace(window.location.pathname, '');
}