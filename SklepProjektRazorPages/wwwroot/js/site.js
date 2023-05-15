// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.
let $cart_counter = document.getElementById('cart-counter');
$counter = 0;
let $cart_layout = document.getElementById("item-counter");
let $DivNoProducts = document.getElementById('display-no-products');

function displayNoProducts(counter) {
    if (counter > 0) {
        if ($DivNoProducts)
            $DivNoProducts.style.display = "none";
    }
}
function updateCart() {
    if ($DivNoProducts) {
        $DivNoProducts.style.display = "block";
        $DivNoProducts.style.visibility = "revert";
        let $OrderSubmit = document.getElementById("order-submit");
        $OrderSubmit.classList.add("prevent-submit");
        $cart_counter.innerHTML = "Koszyk (0)";
        $cart_layout.style.display = "none";
    }
    $counter = 0;
    function genCartProductItem(record) {
        //console.log(record);
        let $container = $(document.createElement('tr'));
        $container.attr("class", "cart-product");
        $container.attr("id", record.product.iD_Produktu);
        $container.html(`<td>${record.product.nazwa}</td><td>${record.amount}szt.<br>(${record.amount * record.product.cena_jednostkowa}zł)</td>`);
        $counter += record.amount;
        let $OrderSubmit = document.getElementById("order-submit");
        displayNoProducts($counter);
        //Amount of products in summary page
        if ($cart_counter) {
            $cart_counter.style.display = "block";
            $cart_counter.textContent = `Koszyk (${$counter})`;
            
        }
        if ($counter >= 1) {
            $cart_layout.style.display = "block";
            $cart_layout.textContent = `${$counter}`;
            if ($OrderSubmit)
                $OrderSubmit.classList.remove("prevent-submit");
        }
        
        return $container[0];
    }
    $.ajax({
        url: '/Cart/api' + "?handler=GetCart",
        method: "POST",
        //contentType: 'application/json'
    }).done(function (data, status, xhr) {
        let $cartList = $("#cart-products");
        $cartList.html('');
        data.products.forEach(productRecord => {
            $cartList.append(genCartProductItem(productRecord));
        });
    });
}
updateCart();

//Adding product animation
function AddCartAnimation() { 
    let $shopCart = document.getElementById('shop-cart');

    $shopCart.classList.toggle("addedClass");
    setTimeout(() => {
        $shopCart.style.transition = "color 1.5s";
        $shopCart.classList.toggle("addedClass");
    }, 1500)
}




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

// Validate if user is logged
function CheckifLogged(argument) {
    if(argument == null)
        return window.location.href = "/Account/Login";
}

// Text-shadow for navbar images
let $CartContainer = document.getElementById("cart-dropdown");
let $account_img = document.getElementById("account-img");
let $CartImg = document.getElementById("shop-cart");
$CartContainer.addEventListener("mouseover", () => {;
    $CartImg.classList.add("TextShadowClass");
});
$CartContainer.addEventListener("mouseout", () => {
    
    $CartImg.classList.remove("TextShadowClass");
});

$account_img.addEventListener("mouseover", () => {
    $account_img.classList.add("TextShadowClass");
})
$account_img.addEventListener("mouseout", () => {
    $account_img.classList.remove("TextShadowClass");
})

//Text-shadow for index products + Button display:visible
let $Card = document.querySelectorAll(".card");
let $CardButton = document.querySelectorAll(".product-cart");
if ($Card) {
    for (let i = 0; i < $Card.length; i++) {
        $Card[i].addEventListener("mouseover", () => {
            $Card[i].classList.add("BoxShadowClass");
            $CardButton[i].style.display = "initial";
        });
        $Card[i].addEventListener("mouseout", () => {
            $Card[i].classList.remove("BoxShadowClass");
            $CardButton[i].style.display = "none";
        });
    }

    
}
//Deactive account
$('#delete-account').confirm({
    title: "Czy na pewno chcesz usunąć konto?",
    content: 'Potwierdzenie spowoduje usunięcie konta, do którego nie będziesz się mógł już zalogować.',
    buttons: {
        deleteUser: {
            text: 'Usuń konto',
            action: function () {
                let $accountID = document.getElementById("delete-account").getAttribute('accountid');
                $.ajax({
                    //url: "/Account/AccountIndex"+`?id=${$accountID}`+"?handler=OnPostDeleteAccount",
                    url: "/Account/AccountIndex?handler=DeleteAccount",
                    type: "POST",
                    datatype:"json",
                    data: {
                        "id": $accountID
                    },
                    success: function() {
                        //alert("udało się");
                    },
                    error: function (status, ex) {
                        alert("Error Code: Status: " + status + " Ex: " + ex);
                        
                    }
                }).done(function (data, status, xhr) {
                    window.location.replace("/Index");
                });
            }
        },
        cancelAction: {
            text: "Cofnij",
            action: function () {

            }
        }
    }
});

//Function to set User's order to 'Zaplacony'

function updateOrder() {
    let OrderID = document.getElementById("updateOrder").getAttribute('orderID');
    //let $Username = document.getElementById("updateOrder").getAttribute('username');
    //let $AccountId = document.getElementById("updateOrder").getAttribute('accountId');
    console.log(OrderID);
    sendAjax("/Account/AccountIndex", "UpdateOrder", { "OrderID": OrderID },).done(function () {
        window.location.reload();
    });
}