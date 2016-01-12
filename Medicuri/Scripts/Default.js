/*Ajax limpio*/
function loadWorkArea(accion, inner) {
    if (window.XMLHttpRequest) { xmlhttp = new XMLHttpRequest(); }
    else { xmlhttp = new ActiveXObject('Microsoft.XMLHTTP'); }
    if (inner) {
        if (xmlhttp.overrideMimeType) { xmlhttp.overrideMimeType('text/html; charset=iso-8859-1'); }
        eval(
   "xmlhttp.onreadystatechange = " +
   " function(){" +
   "  if( xmlhttp.readyState == 4 && xmlhttp.status == 200 ){" +
   "   document.getElementById('" + inner + "').innerHTML = xmlhttp.responseText;" +
   "   js = document.getElementsByTagName( 'script' );" +
   "   for( i = 0 ; i < js.length - 1 ; i++ ){ if( js[ i + 1 ].text != null ){ eval( js[ i + 1 ].text ); } }" +
   "  }" +
   " }"
  );
    }
    xmlhttp.open('POST', accion, true);
    xmlhttp.send();
    return xmlhttp.onreadystatechange;
}


/*Ajax con iframes No POSTBACK en el Master*/
function loadHTMLonDiv(pagina, div) {
    document.getElementById(div).innerHTML = "<iframe frameborder='0' scrolling='no'" +
    "marginheight='0' marginwidth='0' height='100%' width='100%' id='frame' src='" +
    pagina + "' runat='server'></iframe>";
}

function btnLoadPage1_onclick(page) {
    ret = CargaAsincronaHTML.PageLoader.LoadPage(page, OnComplete, OnTimeOut, OnError);
    document.getElementById('Loading').style.display = 'block';
    return (true);
}

function OnComplete(args) {
    document.getElementById('Target').innerHTML = args;
}

function OnTimeOut(args) {
    alert("Service call timed out.");
}

function OnError(args) {
    alert("Error calling service method.");
}


/*Ajax JQuery with POSTBACK*/
function loadDynamic(pagina) {
    $("#content")
        .load(pagina, // URL to call
    // Callback function on completion (optional)
            function (content) {
                // make content visible with effect
                $(this).hide().fadeIn("slow");
                return false;
            });
        }


