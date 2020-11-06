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
    return hours + ":" + minutes + ":" + seconds;
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