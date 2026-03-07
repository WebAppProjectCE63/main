const form = document.getElementById("form");
const fname_input = document.getElementById("fname-input")
const sname_input = document.getElementById("sname-input")
const birthday_input = document.getElementById("birthday-input")
const username_input = document.getElementById("username-input");
const email_input = document.getElementById("email-input");
const password_input = document.getElementById("password-input");
const repeat_password_input = document.getElementById("repeat-password-input");
const error_message = document.getElementById("error-message");
if (birthday_input) {
    birthday_input.max = new Date().toISOString().split("T")[0];
}
let emailExists = false

email_input.addEventListener("blur", () => {

    fetch("/Account/CheckEmail", {
        method: "POST",
        headers: {
            "Content-Type": "application/x-www-form-urlencoded"
        },
        body: "email=" + encodeURIComponent(email_input.value)
    })
        .then(res => res.json())
        .then(data => {

            if (data.exists) {

                emailExists = true

            }
            else {

                emailExists = false

            }

        })

})
async function checkEmail(email) {

    const res = await fetch("/Account/CheckEmail", {
        method: "POST",
        headers: {
            "Content-Type": "application/x-www-form-urlencoded"
        },
        body: "email=" + encodeURIComponent(email)
    })

    const data = await res.json()

    return data.exists
}

form.addEventListener("submit", async (e) => {

    e.preventDefault()   // ⭐ หยุด submit ก่อนเสมอ

    let errors = getSignupFormErrors(
        fname_input.value,
        sname_input.value,
        username_input.value,
        email_input.value,
        password_input.value,
        repeat_password_input.value,
        birthday_input.value
    )

    const exists = await checkEmail(email_input.value)

    if (exists) {
        errors.push("Email already exists")
    }

    if (errors.length > 0) {

        error_message.innerText = errors.join(". ")

    } else {

        form.submit()   // ⭐ ค่อย submit ถ้าไม่มี error

    }

})
function getSignupFormErrors(fname, sname, username, email, password, repeat_password, birthday){
    let errors = []
    const today = new Date()
    const birthDate = new Date(birthday)

    if (fname === "") {
        errors.push("First name required")
    }

    if (sname === "") {
        errors.push("Surname required")
    }

    if (birthday === "") {
        errors.push("Birthday required")
    }
    else if (birthDate > today) {
        errors.push("Birthday cannot be in the future")
        birthday_input.parentElement.classList.add("incorrect")
    }

    if(username === "" || username === null){
        errors.push("Username is required")
        username_input.parentElement.classList.add("incorrect")
    }
    if(email === "" || email === null){
        errors.push("Email is required")
        email_input.parentElement.classList.add("incorrect")
    }
    if(password === "" || password === null){
        errors.push("Password is required")
        password_input.parentElement.classList.add("incorrect")
    }
    if(repeat_password !== password){
        errors.push("Password does not match")
        password_input.parentElement.classList.add("incorrect")
        repeat_password_input.parentElement.classList.add("incorrect")
    }
    return errors;
}

function getLoginFormErrors(email, password){
    let errors = []

    if(email === "" || email === null){
        errors.push("Email is required")
        email_input.parentElement.classList.add("incorrect")
    }
    if(password === "" || password === null){
        errors.push("Password is required")
        password_input.parentElement.classList.add("incorrect")
    }
    return errors;
}

const allInputs = [username_input, email_input, password_input, repeat_password_input].filter(input => input != null)

allInputs.forEach(input => {
    input.addEventListener("input", () => {
        if(input.parentElement.classList.contains("incorrect")){
            input.parentElement.classList.remove("incorrect")
            error_message.innerText = ""
        }
    })
})
function login() {
    $.ajax({
        url: '/Account/LoginAjax',
        type: 'POST',
        data: {
            username: $('#username').val(),
            password: $('#password').val()
        },
        success: function (res) {
            if (res.success)
                window.location = "/Home";
        }
    });
}

if (email_input) {
    email_input.addEventListener("blur", () => {

        fetch("/Account/CheckEmail", {
            method: "POST",
            headers: {
                "Content-Type": "application/x-www-form-urlencoded"
            },
            body: "email=" + encodeURIComponent(email_input.value)
        })
            .then(res => res.json())
            .then(data => {
                emailExists = data.exists
            })

    })
}
