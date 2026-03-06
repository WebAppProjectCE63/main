function openReviewModal(userId, userName, userImage) {
    document.getElementById('reviewTargetUserId').value = userId;
    document.getElementById('reviewTargetName').innerText = userName;
    const imgElement = document.getElementById('reviewTargetImage');
    imgElement.setAttribute("src", userImage ? userImage : "/images/default-avatar.png");
    document.getElementById('reviewModal').style.display = 'flex';
    setRating(0);
}
function openEditReviewModal(userId, userName, userImage) {

}
function closeModal() {
    document.getElementById('reviewModal').style.display = 'none';
}
function setRating(score) {
    document.getElementById('reviewStars').value = score;
    let stars = document.querySelectorAll('.star-rating .star');

    stars.forEach((star, index) => {
        if (index < score) {
            star.classList.add('selected');
        } else {
            star.classList.remove('selected');
        }
    });
}