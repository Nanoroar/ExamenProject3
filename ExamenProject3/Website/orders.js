let listallOrders = document.getElementById('listallOrders');
let loginText = document.getElementById('loginText');



function getOrders() {
    if (!localStorage.getItem('accessToken')) {
        alert('Please log in to see your orders');
        return;
    }

    fetch(`https://examenproject320220622044116.azurewebsites.net/api/OrderRows/Customer/${localStorage.getItem('userId')}`, {
        method: 'get',
        headers: {
            "Content-Type": "application/json",
            "Authorization": `bearer ${localStorage.getItem('accessToken')}`
        }

    }).then(res =>  res.json())
        .then((data) => {
           
            listallOrders.innerHTML = '';
            for (let order of data) {
                listallOrders.innerHTML += `<div class="card rounded-3 text-dark">
            <div class="card-body p-4">
                <div class="row ">
                    
                    <div class="col col-md-3 ">
                        <p class="lead fw-normal mb-2">Order id: ${order.orderId}</p>
                        <p><span class="text-muted">${order.productName}</p>
                    </div>
                    <div class="col col-md-2">
                        <p class="mb-0 price">Articale: ${order.articaleNumber}</p>
                    </div>
                    <div class="col col-md-2">
                        <p class="mb-0 price">Order date: ${order.orderDate}</p>
                    </div>
                    <div class="col col-md-2">
                    <p class="mb-0 price">Quantity: ${order.quantity}</p>
                    </div>
                    <div class="col col-md-2">
                        <h6 class="mb-0 price">Price: ${order.productPrice}</h6>
                    </div>
                    <div class="col-md-1 col-lg-1 col-xl-1 text-end">
                        <a href="#!" class="text-danger px-2" onclick="trash(${order.orderRowId})"><i class="fas fa-trash fa-lg"></i></a>
                    </div>
                </div>
            </div>
            </div>`;
            }
        }).catch(err => {listallOrders.innerHTML = err});
     
        

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

    }
    else
        loginText.innerHTML = 'Log in! <i class="fa fa-user"></i>';
}
confirmLogin();


///=====================================function delete item from order==================

function trash(id) {
    fetch(`https://examenproject320220622044116.azurewebsites.net/api/OrderRows/deleterow/${id}`, {
        method: 'delete',
        headers: {
            "Content-Type": "application/json",
            "Authorization": `bearer ${localStorage.getItem('accessToken')}`
        }
    }).then(res => res.text())
        .then(data => {
            getOrders();
            alert(data);
           
                 

        });
}