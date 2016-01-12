// JavaScript Document
//$(document).ready(function() {
if ($.browser.chrome || $.browser.safari) {
    	document.write('<link rel="stylesheet" type="text/css" href="Css/chrome.css">');
    }else if ($.browser.msie){
        document.write('<link rel="stylesheet" type="text/css" href="Css/ie.css">');
    }else if ($.browser.opera) {
    	document.write('<link rel="stylesheet" type="text/css" href="Css/opera.css">');
    }else if ($.browser.mozilla) {
	    document.write('<link rel="stylesheet" type="text/css" href="Css/firefox.css">');
    }
//}