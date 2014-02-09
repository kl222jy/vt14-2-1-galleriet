//Beräknar och sätter bredden på listan över sparade bilder
width = document.querySelectorAll(".galleryImages img").length * 74;
document.querySelector(".galleryImages").setAttribute("style", "width: " + width + "px");