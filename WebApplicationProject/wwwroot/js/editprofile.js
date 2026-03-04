let originalData = {};

function openEditModal() {
    const modal = document.getElementById("editProfileModal");
    if (!modal) return; 
    originalData = {
        fname: document.getElementById("prevFname")?.innerText || "",
        sname: document.getElementById("prevSname")?.innerText || "",
        email: document.getElementById("prevEmail")?.innerText || "",
        img: document.getElementById("prevImg")?.src || ""
    };
    modal.style.display = "block";
}
function closeEditModal() {
    const modal = document.getElementById("editProfileModal");
    if (!modal) return;
    document.getElementById("prevFname").innerText = originalData.fname;
    document.getElementById("prevSname").innerText = originalData.sname;
    document.getElementById("prevEmail").innerText = originalData.email;
    document.getElementById("prevImg").src = originalData.img;

    modal.style.display = "none";
}

document.addEventListener("DOMContentLoaded", function () {
    const inputFname = document.getElementById("inputFname");
    const inputSname = document.getElementById("inputSname");
    const inputEmail = document.getElementById("inputEmail");
    const inputImg = document.getElementById("inputImg");

    const prevFname = document.getElementById("prevFname");
    const prevSname = document.getElementById("prevSname");
    const prevEmail = document.getElementById("prevEmail");
    const prevImg = document.getElementById("prevImg");

    if (inputFname && prevFname) {
        inputFname.addEventListener("input", function () { prevFname.innerText = this.value || '(ชื่อ)'; });
    }
    if (inputSname && prevSname) {
        inputSname.addEventListener("input", function () { prevSname.innerText = this.value || '(นามสกุล)'; });
    }
    if (inputEmail && prevEmail) {
        inputEmail.addEventListener("input", function () { prevEmail.innerText = this.value || '(Email)'; });
    }
    if (inputImg) {
        inputImg.addEventListener("change", function () {
            const file = this.files[0];
            if (file) {
                const reader = new FileReader();
                reader.onload = function (e) {
                    if (document.getElementById("prevImg")) {
                        document.getElementById("prevImg").src = e.target.result;
                    }
                    
                    const previews = document.querySelectorAll(".preview-img");
                    previews.forEach(img => img.src = e.target.result);
                };
                reader.readAsDataURL(file);
            }
        });
    }
    inputFname.dispatchEvent(new Event('input'));
});