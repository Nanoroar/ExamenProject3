let BaseUrl = 'https://examenproject320220622044116.azurewebsites.net/api';

let loginText = document.getElementById('loginText');
let shopingCartItems = document.getElementById('shopingCartItems');
let proceedToPayButton = document.getElementById('proceedToPay');
var shopingCartEnteries = [];



if (localStorage.getItem('Shopingcart') != null) {
    shopingCartEnteries = JSON.parse(localStorage.getItem('Shopingcart'));

    shoppingItems.innerText = shopingCartEnteries.length;
}


function AdditemsToShopingCart() {
    shopingCartItems.innerHTML = "";
    shopingCartEnteries.forEach((data, index) => {
        shopingCartItems.innerHTML += `
    <div class="card rounded-3 ">
        <div class="card-body p-4">
            <div class="row d-flex justify-content-between align-items-center">
                <div class="col-md-3 col-lg-2 col-xl-2">
                    <img src="./images/products.jpg" class="img-fluid rounded-3" alt="Products pic">
                </div>
                <div class="col-md-3 col-lg-3 col-xl-3">
                    <p class="lead fw-normal mb-2">${data.productName}</p>
                    <p><span class="text-muted">Artical: ${data.articalNumber}</p>
                </div>
                <div class="col-md-3 col-lg-3 col-xl-2 d-flex">
                    <a class="btn btn-link px-2 border buttonminus"
                       onclick="deleteItem(${index})">
                        <i class="fas fa-minus"></i>
                    </a>

                    <input  min="0" name="quantity"  value="${data.quantity}" type="number" class="form-control form-control-sm" />

                    <a class="btn btn-link px-2 border buttonplus" onclick="addQuantity(${index})"><i class="fas fa-plus"></i></a>
                </div>
                <div class="col-sm-3 col-md-3 col-lg-3 col-xl-2 offset-lg-2">
                    <h6 class="mb-0 price">${data.productPrice * data.quantity}</h6>
                </div>
                <div class="col-md-1 col-lg-1 col-xl-1 text-end">
                    <a href="#!" class="text-danger px-2" onclick="trash(${index})"><i class="fas fa-trash fa-lg"></i></a>
                </div>
            </div>
        </div>
    </div>
`;

    });
    shoppingItems.innerText = shopingCartEnteries.length;
    total();
}



AdditemsToShopingCart();

function deleteItem(index) {
    shopingCartEnteries = JSON.parse(localStorage.getItem('Shopingcart'));
    document.querySelectorAll("[type='number']")[index].stepDown();
    shopingCartEnteries[index].quantity = Number(document.getElementsByClassName("form-control form-control-sm")[index].value);

    if (shopingCartEnteries[index].quantity == 0) {

        shopingCartEnteries.splice(index, 1);
    }

    localStorage.setItem('Shopingcart', JSON.stringify(shopingCartEnteries));
    AdditemsToShopingCart();
    shoppingItems.innerText = shopingCartEnteries.length;
    total();
}

function addQuantity(index) {
    shopingCartEnteries = JSON.parse(localStorage.getItem('Shopingcart'));
    document.querySelectorAll("[type='number']")[index].stepUp();
    shopingCartEnteries[index].quantity = Number(document.getElementsByClassName("form-control form-control-sm")[index].value);
    localStorage.setItem('Shopingcart', JSON.stringify(shopingCartEnteries));
    AdditemsToShopingCart();
    total();
}

function trash(index) {
    shopingCartEnteries = JSON.parse(localStorage.getItem('Shopingcart'));
    shopingCartEnteries.splice(index, 1);
    localStorage.setItem('Shopingcart', JSON.stringify(shopingCartEnteries));
    document.querySelectorAll(".text-danger.px-2")[index].parentNode.parentNode.remove;
    AdditemsToShopingCart();
    total();
}

//========================function logout ===================
loginText.addEventListener('click', logOut);
function logOut() {

    if (loginText.innerHTML === 'Log out <i class="fa fa-user"></i>') {
        localStorage.clear();
        confirmLogin();
        sessionStorage.clear();
        loginText.innerHTML = 'Log in <i class="fa fa-user"></i>';
        location.assign('Login.html');
    }
}

//===========================Change log in to log out  ===================
function confirmLogin() {
    if (localStorage.getItem('accessToken') !== 'Incorrect Email or Password' && localStorage.getItem('accessToken') !== null) {
        loginText.innerHTML = 'Log out <i class="fa fa-user"></i>';

    }
    else
        loginText.innerHTML = 'Log in! <i class="fa fa-user"></i>';
}
confirmLogin();
//=====================Check if user loged in Procced to pay button===================================
function checkIfLogin() {
    if (!localStorage.getItem('accessToken')) {
        alert('please sign in to continue');
        return;
    }
    if(!localStorage.getItem('Shopingcart') || localStorage.getItem('Shopingcart') == '[]' ){
        alert('Your order is empty please select products you want to buy and try again!');
        return;
    }
    let userdata = JSON.parse(localStorage.getItem('UserData'));

    let order = {
        customerId: userdata['id'],
        customerName: `${userdata['firstName']} ${userdata['lastName']}`,
        address: `${userdata['streetName']} ${userdata['zipCode']} ${userdata['city']}`,
        orderStatus: "registed"
    }

    let json = JSON.stringify(order);

    fetch(`${BaseUrl}/Orders`, {
        method: 'post',
        headers: {
            "Content-Type": "application/json",
            "Authorization": `bearer ${localStorage.getItem('accessToken')}`
        },
        body: json
    }).then(res => res.json())
    .then((orderid) => {
    
    let orderrows=[];
    let shopingcart = JSON.parse(localStorage.getItem('Shopingcart'));

    for(let item of shopingcart){
        orderrows.push({customerId:Number(localStorage.getItem('userId')), orderId: orderid, articaleNumber:item.articalNumber, productName:item.productName, quantity: item.quantity, productPrice:item.productPrice })
    }
    let rows = JSON.stringify(orderrows);
    fetch(`${BaseUrl}/OrderRows`, {
        method: 'post',
        headers: {
            "Content-Type": "application/json",
            "Authorization": `bearer ${localStorage.getItem('accessToken')}`
        },
        body: rows
    }).then(res => res.text())
    .then(data => {
        proceedToPayButton.classList.add('hidden');
        shopingCartItems.innerHTML =`<h3>${data} , Thanks for buying from ApiStore</h3>`;
        localStorage.removeItem('Shopingcart');
        setTimeout(() => {
           location.assign('index.html') ;
        }, 8000);
    });

    }).catch((err) => {
        loginHeader.textContent = 'OBs something went wrong please try to log in again';
        return;
    });



}


//================================================================================
function total(){
    var sum = 0;
    let pricelements = document.getElementsByClassName('mb-0 price');
    for(let element of pricelements){
        sum += parseFloat(element.textContent);
    }
    document.getElementsByClassName('col total')[0].textContent = 'Total: ' + sum + ":-";
}
