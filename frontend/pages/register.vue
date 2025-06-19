<template>
  <div class="isolate h-screen p-6 lg:px-8">
    <div class="flex justify-end">
      <BaseDarkModeToggle />
    </div>

    <div class="mx-auto mt-16 max-w-md text-center">
      <h2 class="text-3xl font-bold tracking-tight dark:text-white sm:text-4xl">Register</h2>
      <p class="mt-2 text-lg font-semibold leading-8 dark:text-gray-400">
        Create your account to start using ShortURLs
      </p>
    </div>

    <form @submit.prevent="handleRegister" class="mx-auto max-w-md mt-12">
      <div class="space-y-6">
        <div>
          <label for="username" class="block mb-1 text-sm font-medium dark:text-white">Username</label>
          <input
            v-model="form.username"
            type="text"
            id="username"
            required
            class="block w-full p-4 text-sm border rounded-md bg-gray-50 dark:bg-gray-700 dark:text-white dark:border-gray-600"
            placeholder="e.g. johndoe123"
          />
        </div>

        <div>
          <label for="email" class="block mb-1 text-sm font-medium dark:text-white">Email</label>
          <input
            v-model="form.email"
            type="email"
            id="email"
            required
            class="block w-full p-4 text-sm border rounded-md bg-gray-50 dark:bg-gray-700 dark:text-white dark:border-gray-600"
            placeholder="you@example.com"
          />
        </div>

        <div>
          <label for="password" class="block mb-1 text-sm font-medium dark:text-white">Password</label>
          <input
            v-model="form.password"
            type="password"
            id="password"
            required
            class="block w-full p-4 text-sm border rounded-md bg-gray-50 dark:bg-gray-700 dark:text-white dark:border-gray-600"
            placeholder="••••••••"
          />
        </div>

        <div>
          <label for="firstName" class="block mb-1 text-sm font-medium dark:text-white">First Name</label>
          <input
            v-model="form.firstName"
            type="text"
            id="firstName"
            required
            class="block w-full p-4 text-sm border rounded-md bg-gray-50 dark:bg-gray-700 dark:text-white dark:border-gray-600"
            placeholder="John"
          />
        </div>

        <div>
          <label for="lastName" class="block mb-1 text-sm font-medium dark:text-white">Last Name</label>
          <input
            v-model="form.lastName"
            type="text"
            id="lastName"
            required
            class="block w-full p-4 text-sm border rounded-md bg-gray-50 dark:bg-gray-700 dark:text-white dark:border-gray-600"
            placeholder="Doe"
          />
        </div>

        <div class="space-y-3">
          <button
            type="submit"
            :disabled="loading"
            class="w-full flex justify-center items-center px-4 py-3 text-white text-sm font-medium bg-blue-700 hover:bg-blue-800 focus:ring-2 rounded-md dark:bg-blue-600 dark:hover:bg-blue-700"
          >
            <div
              v-if="loading"
              class="w-5 h-5 border-4 border-dashed rounded-full animate-spin dark:border-white"
            ></div>
            <span v-else>Register</span>
          </button>

          <button
            type="button"
            @click="router.push('/login')"
            class="w-full px-4 py-3 text-sm font-medium text-blue-700 bg-white border border-blue-700 rounded-md hover:bg-gray-100 dark:bg-gray-800 dark:border-blue-500 dark:text-blue-400 dark:hover:bg-gray-700"
          >
            Back to Login
          </button>
        </div>
      </div>
    </form>
  </div>
</template>

<script setup>
import { ref } from "vue";
import { useRouter } from "vue-router";
import { useHead } from "#app";
import { API_BASE_URL } from "~/constants";

const router = useRouter();
const loading = ref(false);

const form = ref({
  username: "",
  email: "",
  password: "",
  firstName: "",
  lastName: ""
});

useHead({
  title: "Register | ShortURLs"
});

const handleRegister = async () => {
  loading.value = true;
  try {
    await $fetch(`${API_BASE_URL}user`, {
      method: "POST",
      body: form.value
    });
    alert("Account created! You can now log in.");
    router.push("/login");
  } catch (e) {
    alert(e?.data?.message || "Registration failed.");
  } finally {
    loading.value = false;
  }
};
</script>
