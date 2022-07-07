let BaseUrl = 'https://examenproject320220622044116.azurewebsites.net/api';

const loginHeader = document.getElementById('loginHeader');
const email = document.getElementById('email');
const password = document.getElementById('password');

async function handleLogin(e){
    e.preventDefault();
    let json = JSON.stringify({
        email: email.value,
        password: password.value
    });
const svar = await fetch(`${BaseUrl}/Admins/adminlogin`, {
    method: "post",
    headers: {
        'Content-Type': 'application/json'
    },
    body: json
});

if(svar.ok){
    const data = await svar.json();
    localStorage.setItem('adminkey', data.adminkey);
    localStorage.setItem('adminAccessToken', data.token);
    loginHeader.textContent = "loged in Successfully";
    setTimeout(() => {
       location.assign('CP.html') ;
    }, 3000);
    console.log(data);
}
else{
    const wrongSvar = await svar.text();
    loginHeader.textContent = wrongSvar;
    console.log(wrongSvar);

}

}
