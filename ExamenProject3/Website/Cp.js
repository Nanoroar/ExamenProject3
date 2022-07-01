let productlist = document.getElementById('productlist');

//=======================Create new Product=============================
function handleSubmit(e) {
    e.preventDefault();

    let product = {
        ArticleNumber: e.target["articalenum"].value,
        Name: e.target["name"].value,
        Price: Number(e.target["price"].value),
        CategoryName: e.target["categoryname"].value
    }

    let json = JSON.stringify(product);

    fetch('https://examenproject320220622044116.azurewebsites.net/api/Products', {
        method: 'post',
        headers: {
            'Content-Type': 'application/json',
            'key': '2b25bd2f-3503-4bf3-8831-27d4f2a035b7'
        },
        body: json
    })
        .then(res => res.json())
        .then(data => console.log(data))
        .then(() => { location.reload() })

}
//======================Get All Products CP.html===================================
const getAllProducts = () => {
    fetch('https://examenproject320220622044116.azurewebsites.net/api/Products', {
        method: "Get",
        headers: {
            "Content-Type": "application/json"

        }
    }).then(res => res.json()).then((products) => {
        for (let product of products) {
            productlist.innerHTML +=
                `<li class=" row mt-1 border border-top-0 border-primary">
                        <div class="col mt-2" id="${product.articleNumber}">${product.articleNumber + "&emsp;  " + product.name + "&emsp; " + product.categoryName}</div>
                        <div class=" container col row">
                                      <span class="btn col"> ${product.price + ":-  "}</span>
                                      <button class="btn btn-primary col update" >Update</button>
                                      <button class="btn btn-danger col">Delete</button>
                        </div>
                        <div class="container mt-5 hidden" >
                                 <div class="row g-3">
                                        <div class="col-6">
                                             <form onsubmit="handleUpdate(event)">
                                                  <div class=" mb-3 mt-3">
                                                      <input type="text" name="articalenum" value="${product.articleNumber}"  class="form-control" readonly />
                                                  </div>
                                                  <div class=" mb-3 mt-3">
                                                        <input type="text" name="name" value="${product.name}" class="form-control" placeholder="Product Name" />
                                                  </div>
                                                  <div class=" mb-3">
                                                        <input type="text" name="price" value="${product.price}" class="form-control" placeholder="Price" />
                                                  </div>
                                                  <div class=" mb-3">
                                                        <input type="text" name="categoryname" value="${product.categoryName}" class="form-control" placeholder="Categoryname" />
                                                  </div>

                                                    <button type="submit" class="btn btn-primary form-control">Update</button>
                                             </form>
                                        </div>
                                </div>
                        </div>
    
                </li>`;

        }
    })
}

getAllProducts();




// =========================Function UPDATE Product=================================
function handleUpdate(e) {
    e.preventDefault();

    let artnr = e.target.articalenum.value;
    let product = {
        Name: e.target.name.value,
        Price: Number(e.target.price.value),
        CategoryName: e.target.categoryname.value
    }

    let json = JSON.stringify(product);

    fetch('https://examenproject320220622044116.azurewebsites.net/api/Products/artnr?artnr=' + artnr, {
        method: 'PUT',
        headers: {
            'Content-Type': 'application/json; charset=UTF-8',
            'key': '2b25bd2f-3503-4bf3-8831-27d4f2a035b7'
        },
        body: json

    })
        .then(res => { location.reload() });

}

//=======================================Function delete product CP.html=================
productlist.addEventListener('click', function (e) {

    if (e.target.className == 'btn btn-primary col update') {

        const list = e.target.parentElement.parentElement.childNodes[5].classList;

        list.toggle('hidden');

        if (e.target.innerText == 'Update')
            e.target.innerText = 'Cancle';
        else
            e.target.innerText = 'Update';

    }

    if (e.target.className == 'btn btn-danger col') {
        const artnr = e.target.parentElement.parentElement.childNodes[1].id;

        fetch('https://examenproject320220622044116.azurewebsites.net/api/Products/' + artnr, {
            method: 'Delete',
            headers: {
                'Content-Type': 'application/json; charset=UTF-8',
                'key': '2b25bd2f-3503-4bf3-8831-27d4f2a035b7'
            }

        })
            .then(res => { location.reload() });
    }

})