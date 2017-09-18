function GetTenant() {
    var url = window.location.href;

    var a = url.indexOf("/g-");
    var b = url.indexOf("/", a+1);
    
    var tennantId = url.substring(a+1, b+1);

    return tennantId;
}

function GetXmlHttpRequest(method, url) {
    var xhr = new XMLHttpRequest();

    var tennant = GetTenant();

    var address = Globals.ServerAddr + tennant + "/" + url;
    //console.log("Xhr targer: ", address);
    xhr.open(method, address);

    xhr.setRequestHeader('Access-Control-Allow-Headers', '*');
    xhr.setRequestHeader('Access-Control-Allow-Origin', '*');
    xhr.setRequestHeader('Content-Type', 'application/json; charset=utf-8');

    return xhr;
}