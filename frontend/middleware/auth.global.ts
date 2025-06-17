function isLoggedIn() {
  // check if logged in
  console.log("not logged in!");

  return false;
}

export default defineNuxtRouteMiddleware((to, from) => {
  if (to.name === "login") {
    return;
  }

  // if (isLoggedIn() === false) {
  //   return navigateTo("/login");
  // }
});
