﻿// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Text-shadow for navbar images
let $CartContainer = document.getElementById("cart-dropdown");
let $account_img = document.getElementById("account-img");
let $CartImg = document.getElementById("shop-cart");
$CartContainer.addEventListener("mouseover", () => {
    $CartImg.classList.add("notransition");
    $CartImg.classList.add("navImgClass");
})
$CartContainer.addEventListener("mouseout", () => {
    $CartImg.classList.remove("notransition");
    $CartImg.classList.remove("navImgClass");
})

//Sticky navbar
window.onscroll = function () { scrollingFunction() };

let navbar = document.getElementById("navbar");
let sticky = navbar.offsetTop;

function scrollingFunction() {
    if (window.pageYOffset >= sticky) {
        navbar.classList.add("sticky")
    } else {
        navbar.classList.remove("sticky");
    }
}

$account_img.addEventListener("mouseover", () => {
    $account_img.classList.add("navImgClass");
})
$account_img.addEventListener("mouseout", () => {
    $account_img.classList.remove("navImgClass");
})

function Dropdown() {
    document.getElementById("PopupMenu").classList.toggle("show");
}

window.onclick = function (event) {
    if (!event.target.matches('.drophover')) {

        var dropdowns = document.getElementsByClassName("dropMenu-container");
        var i;
        for (i = 0; i < dropdowns.length; i++) {
            var openDropdown = dropdowns[i];
            if (openDropdown.classList.contains('show')) {
                openDropdown.classList.remove('show');
            }
        }
    }
}
function GenerateDataTables($tableEl) {
    $tableEl = $tableEl instanceof jQuery ? $tableEl : $($tableEl);
    let dataTableOptions = {
        dom: "ltipr",
        searching: true,
        initComplete: function (settings) {
            //console.log("initComplete");
            this.api()
                .columns()
                .every(function () {
                    $("selector").click(false);
                    var that = this;
                    let $input = $("input", this.header());

                    $input.click(false); // prevent sorting when clicked
                    $input.on("keyup change clear ", function () {
                        if (that.search() !== this.value) {
                            that.search(this.value).draw();
                        }
                    });
                });

        },
    };

    return $tableEl.DataTable(dataTableOptions);
};

let $cart_counter = document.getElementById('cart-counter');
let counter = 0;
function updateCart() {
    function genCartProductItem(record) {
        console.log(record);
        let $container = $(document.createElement('tr'));
        $container.attr("class", "cart-product");
        $container.attr("id", record.product.iD_Produktu);
        $container.html(`<td>${record.product.nazwa}</td><td>${record.amount}szt.<br>(${record.amount * record.product.cena_jednostkowa}zł)</td>`);
        //Amount of products in summary page
        if ($cart_counter)
            $cart_counter.textContent = `Koszyk (${counter += record.amount})`;
        return $container[0];
    }
    $.ajax({
        url: '/Cart/api' + "?handler=GetCart",
        method: "POST",
        contentType: 'application/json',
    }).done(function (data, status, xhr) {
        let $cartList = $("#cart-products");
        $cartList.html('');
        data.products.forEach(productRecord => {
            $cartList.append(genCartProductItem(productRecord));
        });
    });
}
updateCart();

let $shopCart = document.getElementById('shop-cart');
let $productCarts = document.querySelectorAll('.product-cart');

$productCarts.forEach(productCart => {
    productCart.addEventListener('click', () => {
        $shopCart.classList.toggle("addedClass");
        setTimeout(() => {
            $shopCart.style.transition = "color 1.5s";
            $shopCart.classList.toggle("addedClass");
        },1500)
    });
});
