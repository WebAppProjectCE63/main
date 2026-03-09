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
function scorrInto(ID) {
    document.getElementById(ID).scrollIntoView({ behavior: 'smooth' });
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

    const privateSwitch = document.getElementById("IsPublic");
    const showEmail = document.getElementById("ShowEmail");
    const showJoined = document.getElementById("ShowJoinedEvents");
    const showHosted = document.getElementById("ShowHostedEvents");

    privateSwitch.addEventListener("change", function () {
        if (privateSwitch.checked) {
            showEmail.checked = false;
            showJoined.checked = false;
            showHosted.checked = false;
        }
        else if (showEmail.checked && showJoined.checked && showHosted.checked) {
            privateSwitch.checked = false;
        }
    });

    function checkSubSwitches() {
        if (showEmail.checked || showJoined.checked && showHosted.checked) {
            privateSwitch.checked = false;
        }
    }

    showEmail.addEventListener("change", checkSubSwitches);
    showJoined.addEventListener("change", checkSubSwitches);
    showHosted.addEventListener("change", checkSubSwitches);

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