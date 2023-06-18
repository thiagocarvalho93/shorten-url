<template>
  <div class="isolate h-screen p-6 lg:px-8">
    <!-- dark mode toggle -->
    <div class="flex justify-end">
      <label class="relative inline-flex items-center cursor-pointer">
        <input type="checkbox" @change="toggleDarkMode" v-model="darkMode" class="sr-only peer" />
        <div
          class="w-11 h-6 bg-gray-200 peer-focus:outline-none peer-focus:ring-4 peer-focus:ring-blue-300 dark:peer-focus:ring-blue-800 rounded-full peer dark:bg-gray-700 peer-checked:after:translate-x-full peer-checked:after:border-white after:content-[''] after:absolute after:top-[2px] after:left-[2px] after:bg-white after:border-gray-300 after:border after:rounded-full after:h-5 after:w-5 after:transition-all dark:border-gray-600 peer-checked:bg-blue-600"
        ></div>
        <span class="ml-3 text-sm font-medium text-gray-900 dark:text-gray-300">Dark mode</span>
      </label>
    </div>
    <!-- title and subtitle -->
    <div class="mx-auto mt-24 max-w-2xl text-center">
      <h2 class="text-3xl font-bold tracking-tight dark:text-white sm:text-4xl">✂ ShortURLs</h2>
      <p class="mt-2 text-lg leading-8 dark:text-gray-400">
        Generate short urls for your long links!
      </p>
    </div>
    <!-- inputs -->
    <form action="#" method="POST" class="mx-auto max-w-xl mt-12">
      <div class="grid grid-cols-1 gap-x-8 gap-y-6 sm:grid-cols-2">
        <div class="sm:col-span-2">
          <div class="relative mb-5">
            <input
              class="block w-full p-4 pr-28 text-sm text-gray-900 border border-gray-300 rounded-md bg-gray-50 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500"
              placeholder="Paste here your long URL"
              v-model="longUrl"
              v-on:keyup.enter="handleGenerate"
              required
              ref="urlInput"
            />
            <button
              class="text-white flex justify-center items-center w-20 absolute right-2.5 bottom-2.5 bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-md text-sm px-4 py-2 dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800 disabled:bg-gray-400"
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
              class="block w-full p-4 pr-28 text-sm text-gray-900 border border-gray-300 rounded-md bg-gray-50 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-900 dark:border-gray-800 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500"
              v-model="completeGeneratedUrl"
              v-on:keyup.enter="handleCopy"
              readonly
            />
            <button
              class="text-blue-700 w-20 absolute right-2.5 bottom-2.5 bg-gray-200 hover:bg-gray-300 focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-md text-sm px-4 py-2 dark:bg-gray-200 dark:hover:bg-gray-300 dark:focus:ring-blue-800"
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
      if (this.darkMode) {
        localStorage.setItem("theme", "dark");
        document.documentElement.classList.add("dark");
      } else {
        localStorage.setItem("theme", "light");
        document.documentElement.classList.remove("dark");
      }
    },
    async handleGenerate() {
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
