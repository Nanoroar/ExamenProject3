
let lielements = document.getElementsByClassName('row mt-1 border border-top-0 border-primary');
let allproducts = document.getElementById('listallproducts');
let shoppingItems = document.getElementById('shoppingItems');
let loginHeader = document.getElementById('loginHeader');
let loginText = document.getElementById('loginText');
const userProfile = document.querySelector("[title='Profile']");
let profileSectionInfo = document.getElementById('profileSectionInfo');
let shopingCartItems = document.getElementById('shopingCartItems')
var shopingCart = [];

if (localStorage.getItem('Shopingcart') != null) {
    shopingCart = JSON.parse(localStorage.getItem('Shopingcart'));

    shoppingItems.innerText = shopingCart.length;
}


//=========================fetch Profile info to Profile.html=====================

function getProfileInfo() {
    if (!localStorage.getItem('accessToken') && !localStorage.getItem('userId')) {
        profileSectionInfo.innerHTML = `<section class="h-100 mt-3 "  style="background-color: #eee;">
        <div class="container h-100 py-5">
          <div class="row d-flex justify-content-center align-items-center h-100">
            <div class="col-10"> 
              <div class="d-flex justify-content-between align-items-center mb-4">
                <h3 class="fw-normal mb-0 text-black">User profile</h3>     
              </div>
              <div class="card rounded-3 mb-4">
                <div class="card-body p-4">
                  <div class="row d-flex justify-content-start align-items-center">               
                    <div class="col">
                      <p class="lead fw-normal mb-2">Please log in to access this page</p>            
                    </div>                           
                  </div>
                </div>
              </div>
              </section>`;
        return;
    }

    fetch(`https://localhost:7047/api/Users/${localStorage.getItem('userId')}`, {
        method: "get",
        headers: {
            "Content-Type": "application/json",
            "Authorization": `bearer ${localStorage.getItem('accessToken')}`
        }
    })
        .then(res => res.json())
        .then((data) => {
            localStorage.setItem('UserData', JSON.stringify(data));
            document.getElementById("fname").value = data.firstName;
            document.getElementById("lname").value = data.lastName;
            document.getElementById("e-mail").value = data.email;
            document.getElementById("phone-number").value = data.phoneNumber;
            document.getElementById("street-name").value = data.streetName;
            document.getElementById("postal-code").value = data.zipCode;
            document.getElementById("cty").value = data.city;

        });
}
//=================================function Update profile information =================
function UpdateProfileinfo(e) {
    e.preventDefault();
    let user = {
        phoneNumber: e.target['phone-number'].value,
        streetName: e.target['street-name'].value,
        PostalCode: e.target['postal-code'].value,
        city: e.target['cty'].value
    };

    let json = JSON.stringify(user);

    fetch(`https://localhost:7047/api/Users/update/${localStorage.getItem('userId')}`, {
        method: 'put',
        headers: {
            "Content-Type": "application/json",
            "Authorization": `bearer ${localStorage.getItem('accessToken')}`
        },
        body: json
    }).then(() => location.reload());
}
//=========================function delete account====================
function DeleteProfile() {
    let r = confirm('Are you sure? you are going to delete your account?')
    if (r) {
        fetch(`https://localhost:7047/api/Users/Delete/${localStorage.getItem('userId')}`, {
            method: 'delete',
            headers: {
                "Content-Type": "application/json",
                "Authorization": `bearer ${localStorage.getItem('accessToken')}`
            }

        })
            .then(() => localStorage.clear())
            .then(() => alert('You account is now deleted!'))
            .then(() => location.reload());

    }
}
//========================function logout ===================
loginText.addEventListener('click', logOut);
function logOut() {

    if (loginText.innerHTML === 'Log out <i class="fa fa-user" aria-hidden="true"></i>') {
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
        getProfileInfo();
    }
    else
        loginText.innerHTML = 'Log in! <i class="fa fa-user"></i>';
}
confirmLogin();
//=====================Check if user loged in Procced to pay button===================================
function checkIfLogin() {
    if (localStorage.getItem('accessToken'))
        alert('you are signed in');
    else alert('please sign in to continue');
}


//======================Get All Products index.html===================================
const showAllProducts = () => {
    fetch('https://localhost:7047/api/Products', {
        method: "Get",
        headers: {
            "Content-Type": "application/json"

        }
    }).then(x => x.json()).then((products) => {
        for (let product of products) {
            allproducts.innerHTML +=
                `<div class=" col-md-4 flex-wrap mb-5 text-center shadow-sm" id="${product.articleNumber}">
                
                <img  alt="Products image" class="img-fluid rounded"  src="./images/products.jpg" />
                <h4 class="text-dark text-center mt-2" name="pname">${product.name}</h4>
                <p class="text-dark text-center" name="pcatagory">${product.categoryName}</p>
                <p class="text-dark text-center" name="partical">${product.articleNumber}</p>
                <p class="text-dark text-center" value="${Number(product.price)}" name="pprice">${product.price}</p>
               
                <button type="submit" class="btn btn-primary mb-3 col-4" >Add to cart</button>
            
            </div>`;


        }
    }).then(() => {
        let buttons = document.getElementsByClassName("btn btn-primary mb-3 col-4");
        for (let button of buttons) {
            button.addEventListener('click', function buyProduct(e) {
                const price = e.target.parentElement.querySelector('[name="pprice"]').textContent;
                let product = {
                    articalNumber: e.target.parentElement.querySelector('[name="partical"]').textContent,
                    productName: e.target.parentElement.querySelector('[name="pname"]').textContent,
                    quantity: 1,
                    productPrice: Number(price)
                }

                shopingCart.push(product);
                localStorage.setItem('Shopingcart', JSON.stringify(shopingCart));

                shopingCart =JSON.parse(localStorage.getItem('Shopingcart'));

                shoppingItems.innerText = shopingCart.length;


            });
        }
    });
}

showAllProducts();

//====================================Signup form add user ===============================

function handleSignUp(e) {
    e.preventDefault();
    let user = {
        FirstName: e.target['firstname'].value,
        LastName: e.target['lastname'].value,
        Email: e.target['email'].value,
        PhoneNumber: e.target['phonenumber'].value,
        StreetName: e.target['streetname'].value,
        PostalCode: e.target['postalcode'].value,
        City: e.target['city'].value,
        Password: e.target['password'].value
    }

    let json = JSON.stringify(user);

    fetch('https://localhost:7047/api/users/signup', {
        method: 'post',
        headers: { "Content-Type": "application/json" },
        body: json
    }).then((response) => {
        if (response.status === 200) {
            document.getElementById('responce').innerHTML = "User Created Successfully";
            document.querySelector(".card").classList.add("responceOk");
            setTimeout(() => {
                location.assign('Login.html');
            }, 4000);
        } else {
            document.getElementById('responce').innerHTML = "Error, this email address is taken";
            document.querySelector(".card").classList.add("error");
        }
    })
}

//==========================================Sign in form user Login.html====================================

function handleLogin(e) {
    e.preventDefault();
    const inLogInfo = {
        Email: e.target['email'].value,
        Password: e.target['password'].value
    }
    json = JSON.stringify(inLogInfo);

    fetch('https://localhost:7047/api/users/signin', {
        method: 'post',
        headers: { "Content-Type": "application/json" },
        body: json
    }).then(res => res.json())
        .then((data) => {

            localStorage.setItem('accessToken', data.accessToken);
            localStorage.setItem('userId', data.id);
            if (localStorage.getItem('accessToken') !== null) {
                loginHeader.textContent = 'Loged in successfully!';
                setTimeout(() => { location.assign('index.html') }, 2000);
            } else {
                loginHeader.textContent = 'Wrong email or password';
                return;
            }
        }).catch((err) => {
            loginHeader.textContent = 'Wrong email or password';
            return;
        });
}





///==========================function add items to the shoping cart shopingCart.html==============================
