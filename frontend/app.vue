<template>
  <div class="isolate h-screen p-6 lg:px-8">
    <!-- dark mode toggle -->
    <div class="flex justify-end">
      <button
        id="theme-toggle"
        type="button"
        class="transition duration-500 ease-in-out text-gray-500 dark:text-gray-400 hover:bg-gray-200 dark:hover:bg-slate-900 focus:outline-none focus:ring-1 focus:ring-gray-200 dark:focus:ring-gray-700 rounded-lg text-sm p-2.5"
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
      <!-- <label class="relative inline-flex items-center cursor-pointer">
        <input type="checkbox" @change="toggleDarkMode" v-model="darkMode" class="sr-only peer" />
        <div
          class="w-11 h-6 bg-gray-200 peer-focus:outline-none peer-focus:ring-4 peer-focus:ring-blue-300 dark:peer-focus:ring-blue-800 rounded-full peer dark:bg-gray-700 peer-checked:after:translate-x-full peer-checked:after:border-white after:content-[''] after:absolute after:top-[2px] after:left-[2px] after:bg-white after:border-gray-300 after:border after:rounded-full after:h-5 after:w-5 after:transition-all dark:border-gray-600 peer-checked:bg-blue-600"
        ></div>
        <span class="ml-3 text-sm font-medium text-gray-900 dark:text-gray-300">Dark mode</span>
      </label> -->
    </div>
    <!-- title and subtitle -->
    <div class="mx-auto mt-24 max-w-2xl text-center">
      <h2
        class="transition duration-500 ease-in-out text-3xl font-bold tracking-tight dark:text-white sm:text-4xl"
      >
        ✂ ShortURLs
      </h2>
      <p class="transition duration-500 ease-in-out mt-2 text-lg leading-8 dark:text-gray-400">
        Generate short urls for your long links!
      </p>
    </div>
    <!-- inputs -->
    <form action="#" method="POST" class="mx-auto max-w-xl mt-12">
      <div class="grid grid-cols-1 gap-x-8 gap-y-6 sm:grid-cols-2">
        <div class="sm:col-span-2">
          <div class="relative mb-5">
            <input
              class="transition duration-500 ease-in-out block w-full p-4 pr-28 text-sm text-gray-900 border border-gray-300 rounded-md bg-gray-50 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500"
              placeholder="Paste here your long URL"
              v-model="longUrl"
              v-on:keyup.enter="handleGenerate"
              required
              ref="urlInput"
            />
            <button
              class="transition duration-500 ease-in-out text-white flex justify-center items-center w-20 absolute right-2.5 bottom-2.5 bg-blue-700 hover:bg-blue-800 focus:ring-2 focus:outline-none focus:ring-blue-300 font-medium rounded-md text-sm px-4 py-2 dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800 disabled:bg-gray-400 dark:disabled:bg-gray-400"
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
              class="transition duration-500 ease-in-out block w-full p-4 pr-28 text-sm text-gray-900 border border-gray-300 rounded-md bg-gray-200 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-900 dark:border-gray-800 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500"
              v-model="completeGeneratedUrl"
              v-on:keyup.enter="handleCopy"
              readonly
            />
            <button
              class="transition duration-500 ease-in-out text-blue-700 w-20 absolute right-2.5 bottom-2.5 bg-white hover:bg-gray-300 focus:ring-2 focus:outline-none focus:ring-blue-300 font-medium rounded-md text-sm px-4 py-2 dark:bg-gray-200 dark:hover:bg-gray-300 dark:focus:ring-blue-800"
              type="button"
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

<script>
import { API_BASE_URL } from "./constants";
import { initFlowbite } from "flowbite";

export default {
  name: "IndexPage",
  data() {
    return {
      baseApiUrl: API_BASE_URL,
      longUrl: "",
      generatedUrl: "",
      loading: false,
      darkMode: false,
    };
  },

  mounted() {
    initFlowbite();
    this.checkDarkMode();
    this.$refs.urlInput.focus();
  },

  computed: {
    completeGeneratedUrl() {
      return this.generatedUrl.token ? `${this.baseApiUrl}/${this.generatedUrl.token}` : "";
    },

    invalidUrl() {
      const urlRegex =
        /((([A-Za-z]{3,9}:(?:\/\/)?)(?:[-;:&=\+\$,\w]+@)?[A-Za-z0-9.-]+|(?:www.|[-;:&=\+\$,\w]+@)[A-Za-z0-9.-]+)((?:\/[\+~%\/.\w-_]*)?\??(?:[-\+=&;%@.\w_]*)#?(?:[\w]*))?)/;
      return this.longUrl.length < 1 || !this.longUrl.match(urlRegex);
    },
  },
  methods: {
    checkDarkMode() {
      if (
        localStorage.theme === "dark" ||
        (!("theme" in localStorage) && window.matchMedia("(prefers-color-scheme: dark)").matches)
      ) {
        this.darkMode = true;
        document.documentElement.classList.add("dark");
      } else {
        document.documentElement.classList.remove("dark");
      }
    },

    toggleDarkMode() {
      this.darkMode = !this.darkMode;
      if (this.darkMode) {
        localStorage.setItem("theme", "dark");
        document.documentElement.classList.add("dark");
      } else {
        localStorage.setItem("theme", "light");
        document.documentElement.classList.remove("dark");
      }
    },
    async handleGenerate() {
      if (this.invalidUrl) return;
      this.loading = true;
      try {
        const data = await this.postUrl();
        console.log(data);
        this.generatedUrl = data;
      } catch (e) {
        const message = e.data?.errors ?? "Error connecting to service!"; //TODO
        alert(message);
      } finally {
        this.loading = false;
      }
    },

    handleCopy() {
      navigator.clipboard.writeText(this.completeGeneratedUrl);
      alert("Copied to clipboard!");
    },

    async postUrl() {
      const response = await $fetch(API_BASE_URL, {
        method: "POST",
        body: {
          url: this.longUrl,
        },
      });
      return response;
    },
  },
};
</script>
