var searchBar = document.getElementById("searchBar");
var brandCollapseBtn = document.getElementById("collapsibleBrands");
var categCollapseBtn = document.getElementById("collapsibleCategs");



//Make Brands button collapse options
function initBrandFilterBtn() {
    var trigger = brandCollapseBtn.querySelectorAll(".bigThingie")[0];
    var brandCollapsibles = brandCollapseBtn.querySelectorAll(".collapse")[0];

    trigger.addEventListener("click", function () {
        $(brandCollapsibles).collapse("toggle"); /*imi recunosc pacatele*/
    });
};
//Make Categories button collapse options
function initCategFilterBtn() {
    var trigger = categCollapseBtn.querySelectorAll(".bigThingie")[0];
    var categCollapsibles = categCollapseBtn.querySelectorAll(".collapse")[0];

    trigger.addEventListener("click", function () {
        $(categCollapsibles).collapse("toggle"); /*imi recunosc pacatele*/
    });
};
//Search Bar keyup triggers search
function initSearchBar() {
    searchBar.addEventListener("keyup", function () {
        RefreshProducts();
    })
};

//Makes Brands and Categories selectable and activates filtering mechanix
function initFilters() {
    //Brands
    var options = brandCollapseBtn.getElementsByClassName("smallThingies")[0].children;
    for (var i = 0; i < options.length; i++) {
        options[i].isTriggered = false;
        options[i].addEventListener("click", FilterToggle);
    }
    //Categories
    options = categCollapseBtn.getElementsByClassName("smallThingies")[0].children;
    for (i = 0; i < options.length; i++) {
        options[i].isTriggered = false;
        options[i].addEventListener("click", FilterToggle);
    }
};
//Clicking on a filter
function FilterToggle(evt) {
    var filterObj = evt.target;
    if (filterObj.isTriggered) {
        filterObj.isTriggered = false;
        filterObj.style.backgroundColor = "white";
    } else {
        filterObj.isTriggered = true;
        filterObj.style.backgroundColor = "#b3daff";
    }
    RefreshProducts();
};
//Gather filtering data
function RefreshProducts() {
    var BrandFilters = [];
    var CategoryFilters = [];

    var options = categCollapseBtn.getElementsByClassName("smallThingies")[0].children;
    for (var i = 0; i < options.length; i++) {
        if (options[i].isTriggered) {
            BrandFilters.push(options[i].innerHTML);
        }
    }
    options = brandCollapseBtn.getElementsByClassName("smallThingies")[0].children;
    for (i = 0; i < options.length; i++) {
        if (options[i].isTriggered) {
            CategoryFilters.push(options[i].innerHTML);
        }
    }
    FilterProducts(searchBar.value, CategoryFilters, BrandFilters);
};

function FilterProducts(searchText, categories, brands) {

    if (searchText === null) {
        searchText = "";
    }

    var obj = new Object();

    obj.SearchString = searchText
    obj.Categories = [];
    for (var i = 0; i < categories.length; i++) {
        obj.Categories.push({ Name: categories[i] });
    }

    obj.Brands = [];
    for (i = 0; i < brands.length; i++) {
        obj.Brands.push({ Name: brands[i] });
    }

    obj = JSON.stringify(obj);

    var xhr = GetXmlHttpRequest('POST', 'Product/ProductListPartial/?filters=' + encodeURIComponent(obj))

    xhr.onload = function () {
        if (xhr.status === 200) {
            var ProductGrid = document.getElementsByClassName("ProductGrid")[0];
            ProductGrid.innerHTML = xhr.response;
        }
        else {
            alert('Filter Products Request failed.  Returned status of ' + xhr.status);
        }
    };
    xhr.send();
}

//This one initializes everything related to the filters
function initFilterBar() {
    initSearchBar();
    initCategFilterBtn();
    initBrandFilterBtn();

    initFilters();
};
initFilterBar();