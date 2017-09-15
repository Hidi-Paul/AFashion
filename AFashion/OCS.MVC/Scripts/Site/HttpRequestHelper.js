function GetTenant() {
    var url = window.location.href;
    alert(url);
    console.log("URL:", url)
}

function GetXmlHttpRequest(method, url) {
    var xhr = new XMLHttpRequest();

    var tennant = GetTenant();

    xhr.open(method, Globals.ServerAddr + tennant + "/" + url);

    xhr.setRequestHeader('Access-Control-Allow-Headers', '*');
    xhr.setRequestHeader('Access-Control-Allow-Origin', '*');
    xhr.setRequestHeader('Content-Type', 'application/json; charset=utf-8');

    return xhr;
}