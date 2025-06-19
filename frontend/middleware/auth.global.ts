function isLoggedIn() {
  const token = localStorage.getItem("token");

  if (token) {
    return true;
  }
  return false;
}

export default defineNuxtRouteMiddleware((to, from) => {
  if (to.name === "login") {
    return;
  }

  if (isLoggedIn() === false) {
    return navigateTo("/login");
  }
});
