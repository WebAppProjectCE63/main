let originalData = {};
document.addEventListener("DOMContentLoaded", function () {
    const inputTitle = document.getElementById("inputTitle");
    const inputDesc = document.getElementById("inputDesc");
    const inputImg = document.getElementById("inputImg");
    const inputTag = document.getElementById("inputTag");
    const inputMaxParti = document.getElementById("inputMaxParti");
    const inputMaxWait = document.getElementById("inputMaxWait");
    const inputDateTime = document.getElementById("inputDateTime");
    const inputEndDateTime = document.getElementById("inputEndDateTime");
    const inputLocation = document.getElementById("inputLocation");

    const prevImage = document.getElementById("prevImage");
    const prevTitle = document.getElementById("prevTitle");
    const prevDesc = document.getElementById("prevDesc");
    const prevTags = document.getElementById("prevTags");
    const prevMaxParti = document.getElementById("prevMaxParti");
    const prevDate = document.getElementById("prevDate");
    const prevLocation = document.getElementById("prevLocation");

    const resetbutton = document.getElementById("btnRst");

    originalData = {
        Title: inputTitle.value,
        Desc: inputDesc.value,
        Img: document.getElementById("oldImageUrl") ? document.getElementById("oldImageUrl").value : "",
        Tag: inputTag.value,
        MaxParti: inputMaxParti.value,
        MaxWait: inputMaxWait.value,
        DateTime: inputDateTime.value,
        EndDateTime: inputEndDateTime ? inputEndDateTime.value : "",
        Location: inputLocation.value,
    };

    inputTitle.addEventListener("input", function () {
        if (!this.value) {
            prevTitle.innerHTML = '(ชื่อกิจกรรมจะปรากฏที่นี่)';
            return;
        }
        prevTitle.innerHTML = this.value
    });

    inputDesc.addEventListener("input", function () {
        if (!this.value) {
            prevDesc.innerHTML = 'รายละเอียดต่างๆ ของกิจกรรม...';
            return;
        }
        prevDesc.innerHTML = this.value
    });

    inputImg.addEventListener("change", function () {
        const file = this.files[0];

        if (file) {
            prevImage.src = URL.createObjectURL(file);
        } else {
            prevImage.src = "https://img2.pic.in.th/image-icon-symbol-design-illustration-vector.md.jpg";
        }
    });

    inputTag.addEventListener("input", function () {
        let tagText = this.value
        if (!tagText) {
            prevTags.innerHTML = '<span class="tag-badge">ตัวอย่างแท็ก</span>';
            return;
        }
        let badges = tagText.split(',').map(tag => `<span class="tag-badge">${tag.trim()}</span>`).join(" ");
        prevTags.innerHTML = badges;
    });
    function updateParticipantInfo() {
        if (inputMaxParti.value && parseInt(inputMaxParti.value) > 0) {
            inputMaxWait.disabled = false;
            let maxAllowed= Math.ceil(parseInt(inputMaxParti.value) / 2);
            inputMaxWait.max = maxAllowed;
            inputMaxWait.placeholder = `สูงสุด ${maxAllowed} คน`;
            if (inputMaxWait.value && parseInt(inputMaxWait.value) > maxAllowed) {
                inputMaxWait.value = maxAllowed;
            }
        } else {
            inputMaxWait.disabled = true;
            inputMaxWait.value = "";
            inputMaxWait.placeholder = "กรอกจำนวนตัวจริงก่อน";
        }
        if (!inputMaxParti.value) {
            prevMaxParti.innerHTML = 'XX';
        } else {
            let waitText = inputMaxWait && inputMaxWait.value ? ` (สำรอง ${inputMaxWait.value})` : "";
            prevMaxParti.innerHTML = inputMaxParti.value + waitText;
        }
    }

    inputMaxParti.addEventListener("input", updateParticipantInfo);
    if (inputMaxWait) {
        inputMaxWait.addEventListener("input", updateParticipantInfo);
    }
    if (inputMaxParti && inputMaxParti.value) {
        updateParticipantInfo();
    }

    function updateDatePreview() {
        if (!inputDateTime.value) {
            prevDate.innerHTML = 'XX เดือน XXXX - XX:XX น.';
            return;
        }

        const startObj = new Date(inputDateTime.value);
        const startDate = startObj.toLocaleDateString('en-GB', { day: 'numeric', month: 'long', year: 'numeric' });
        const startTime = startObj.toLocaleTimeString('en-GB', { hour: '2-digit', minute: '2-digit' });

        if (!inputEndDateTime || !inputEndDateTime.value) {
            prevDate.innerHTML = `${startDate} ( ${startTime} น. )`;
            return;
        }

        const endObj = new Date(inputEndDateTime.value);
        const endDate = endObj.toLocaleDateString('en-GB', { day: 'numeric', month: 'long', year: 'numeric' });
        const endTime = endObj.toLocaleTimeString('en-GB', { hour: '2-digit', minute: '2-digit' });


        if (startDate === endDate) {
            prevDate.innerHTML = `${startDate} ( ${startTime} - ${endTime} )`;
        } else {
            prevDate.innerHTML = `${startDate} ( ${startTime} ) - ${endDate} ( ${endTime} )`;
        }
    }

    inputDateTime.addEventListener("input", function () {
        if (this.value) {
            inputEndDateTime.disabled = false;
            inputEndDateTime.min = this.value;
            if (inputEndDateTime.value && inputEndDateTime.value < this.value) {
                inputEndDateTime.value = "";
            }
        } else {
            inputEndDateTime.disabled = true;
            inputEndDateTime.value = "";
        }
        updateDatePreview();
    });


    if (inputEndDateTime) {
        inputEndDateTime.addEventListener("input", updateDatePreview);
    }

    if (inputDateTime && inputEndDateTime && inputDateTime.value) {
        inputEndDateTime.min = inputDateTime.value;
        inputEndDateTime.disabled = false;
        updateDatePreview();
    }

    inputLocation.addEventListener("input", function () {
        if (!this.value) {
            prevLocation.innerHTML = 'ระบุสถานที่จัดงาน';
            return;
        }
        prevLocation.innerHTML = this.value
    });

    resetbutton.addEventListener("click", function () {
        if (inputTitle) inputTitle.value = originalData.Title;
        if (inputDesc) inputDesc.value = originalData.Desc;
        if (inputTag) inputTag.value = originalData.Tag;
        if (inputMaxParti) inputMaxParti.value = originalData.MaxParti;
        if (inputMaxWait) inputMaxWait.value = originalData.MaxWait;
        if (inputDateTime) inputDateTime.value = originalData.DateTime;
        if (inputEndDateTime) inputEndDateTime.value = originalData.EndDateTime;
        if (inputLocation) inputLocation.value = originalData.Location;
        if (inputImg) {
            inputImg.value = "";
        }
        if (prevImage) {
            prevImage.src = originalData.Img ? originalData.Img : "https://img2.pic.in.th/image-icon-symbol-design-illustration-vector.md.jpg";
        }
        const event = new Event('input');
        if (inputTitle) inputTitle.dispatchEvent(event);
        if (inputDesc) inputDesc.dispatchEvent(event);
        if (inputTag) inputTag.dispatchEvent(event);
        if (inputMaxParti) inputMaxParti.dispatchEvent(event);
        if (inputMaxWait) inputMaxWait.dispatchEvent(event);
        if (inputDateTime) inputDateTime.dispatchEvent(event);
        if (inputEndDateTime) inputEndDateTime.dispatchEvent(event);
        if (inputLocation) inputLocation.dispatchEvent(event);
    })
    if (inputTag) inputTag.dispatchEvent(new Event('input'));
});