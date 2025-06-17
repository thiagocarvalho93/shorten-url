<template>
  <div class="isolate h-screen p-6 lg:px-8">
    <div class="flex justify-end">
      <button
        type="button"
        class="transition duration-500 ease-in-out text-gray-500 dark:text-gray-400 hover:bg-gray-200 dark:hover:bg-slate-900 focus:outline-none focus:ring-1 focus:ring-gray-200 dark:focus:ring-slate-900 rounded-lg text-sm p-2.5"
        @click="toggleDarkMode"
      >
        <svg
          id="theme-toggle-dark-icon"
          v-if="!darkMode"
          class="w-5 h-5"
          fill="currentColor"
          viewBox="0 0 20 20"
          xmlns="http://www.w3.org/2000/svg"
        >
          <path d="M17.293 13.293A8 8 0 016.707 2.707a8.001 8.001 0 1010.586 10.586z"></path>
        </svg>
        <svg
          v-else
          id="theme-toggle-light-icon"
          class="w-5 h-5"
          fill="currentColor"
          viewBox="0 0 20 20"
          xmlns="http://www.w3.org/2000/svg"
        >
          <path
            d="M10 2a1 1 0 011 1v1a1 1 0 11-2 0V3a1 1 0 011-1zm4 8a4 4 0 11-8 0 4 4 0 018 0zm-.464 4.95l.707.707a1 1 0 001.414-1.414l-.707-.707a1 1 0 00-1.414 1.414zm2.12-10.607a1 1 0 010 1.414l-.706.707a1 1 0 11-1.414-1.414l.707-.707a1 1 0 011.414 0zM17 11a1 1 0 100-2h-1a1 1 0 100 2h1zm-7 4a1 1 0 011 1v1a1 1 0 11-2 0v-1a1 1 0 011-1zM5.05 6.464A1 1 0 106.465 5.05l-.708-.707a1 1 0 00-1.414 1.414l.707.707zm1.414 8.486l-.707.707a1 1 0 01-1.414-1.414l.707-.707a1 1 0 011.414 1.414zM4 11a1 1 0 100-2H3a1 1 0 000 2h1z"
            fill-rule="evenodd"
            clip-rule="evenodd"
          ></path>
        </svg>
      </button>
    </div>

    <div class="mx-auto mt-24 max-w-2xl text-center">
      <h2 class="text-3xl font-bold tracking-tight dark:text-white sm:text-4xl">✂ ShortURLs</h2>
      <p class="mt-2 text-lg font-semibold leading-8 dark:text-gray-400">
        Generate short urls for your long links!
      </p>
    </div>

    <form @submit.prevent class="mx-auto max-w-xl mt-12">
      <div class="grid grid-cols-1 gap-x-8 gap-y-6 sm:grid-cols-2">
        <div class="sm:col-span-2">
          <div class="relative mb-5">
            <input
              ref="urlInput"
              v-model="longUrl"
              @keyup.enter="handleGenerate"
              class="block w-full p-4 pr-28 text-sm border rounded-md bg-gray-50 dark:bg-gray-700 dark:text-white dark:border-gray-600"
              placeholder="Paste here your long URL"
              required
            />
            <button
              class="text-white flex justify-center items-center w-20 absolute right-2.5 bottom-2.5 bg-blue-700 hover:bg-blue-800 focus:ring-2 rounded-md text-sm px-4 py-2 dark:bg-blue-600 dark:hover:bg-blue-700"
              type="button"
              @click="handleGenerate"
              :disabled="invalidUrl"
            >
              <div
                v-if="loading"
                class="w-5 h-5 border-4 border-dashed rounded-full animate-spin dark:border-white"
              ></div>
              <span v-else>Go</span>
            </button>
          </div>

          <div v-if="generatedUrl.token" class="relative">
            <input
              readonly
              v-model="completeGeneratedUrl"
              @keyup.enter="handleCopy"
              class="block w-full p-4 pr-28 text-sm rounded-md bg-gray-200 dark:bg-gray-900 dark:text-white"
            />
            <button
              type="button"
              class="text-blue-700 w-20 absolute right-2.5 bottom-2.5 bg-white hover:bg-gray-300 dark:bg-gray-200 dark:hover:bg-gray-300 rounded-md text-sm px-4 py-2"
              @click="handleCopy"
            >
              Copy
            </button>
          </div>
        </div>
      </div>
    </form>
  </div>
</template>

<script setup>
import { onMounted, ref, computed } from "vue";
import { useHead } from "#app";
import { API_BASE_URL } from "@/constants";

const longUrl = ref("");
const generatedUrl = ref({});
const loading = ref(false);
const darkMode = ref(false);
const urlInput = ref(null);

const urlRegex = new RegExp(
  /((([A-Za-z]{3,9}:(?:\/\/)?)(?:[-;:&=\+\$,\w]+@)?[A-Za-z0-9.-]+|(?:www.|[-;:&=\+\$,\w]+@)[A-Za-z0-9.-]+)((?:\/[\+~%\/.\w-_]*)?\??(?:[-\+=&;%@.\w_]*)#?(?:[\w]*))?)/
);

const invalidUrl = computed(() => !urlRegex.test(longUrl.value));

const completeGeneratedUrl = computed(() =>
  generatedUrl.value.token ? `${API_BASE_URL}/${generatedUrl.value.token}` : ""
);

const checkDarkMode = () => {
  const prefersDark = window.matchMedia("(prefers-color-scheme: dark)").matches;
  darkMode.value = localStorage.theme === "dark" || (!localStorage.theme && prefersDark);
  document.documentElement.classList.toggle("dark", darkMode.value);
};

const toggleDarkMode = () => {
  darkMode.value = !darkMode.value;
  localStorage.theme = darkMode.value ? "dark" : "light";
  document.documentElement.classList.toggle("dark", darkMode.value);
};

const handleGenerate = async () => {
  console.log(invalidUrl.value);
  
  if (invalidUrl.value) return;
  loading.value = true;
  try {
    const response = await $fetch(API_BASE_URL, {
      method: "POST",
      body: { url: longUrl.value },
    });
    generatedUrl.value = response;
  } catch (e) {
    alert(e?.data?.errors || "Error connecting to service!");
  } finally {
    loading.value = false;
  }
};

const handleCopy = async () => {
  try {
    await navigator.clipboard.writeText(completeGeneratedUrl.value);
    alert("Copied!");
  } catch {
    alert("Cannot copy.");
  }
};

onMounted(() => {
  checkDarkMode();
  urlInput.value?.focus();
});
</script>
