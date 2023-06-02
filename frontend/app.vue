<template>
  <div class="isolate h-screen px-6 py-24 sm:py-32 lg:px-8">
    <div class="mx-auto max-w-2xl text-center">
      <h2 class="text-3xl font-bold tracking-tight text-white sm:text-4xl">✂ShortURLs</h2>
      <p class="mt-2 text-lg leading-8 text-gray-600">Generate short urls for your long links!</p>
    </div>
    <form action="#" method="POST" class="mx-auto mt-16 max-w-xl sm:mt-20">
      <div class="grid grid-cols-1 gap-x-8 gap-y-6 sm:grid-cols-2">
        <div class="sm:col-span-2">
          <div class="relative mb-5">
            <input
              class="block w-full p-4 pr-28 text-sm text-gray-900 border border-gray-300 rounded-md bg-gray-50 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500"
              placeholder="Paste here your long URL"
              v-model="longUrl"
              required
              ref="urlInput"
            />
            <button
              class="text-white w-20 absolute right-2.5 bottom-2.5 bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-md text-sm px-4 py-2 dark:bg-blue-600 dark:hover:bg-blue-700 dark:focus:ring-blue-800 dark:disabled:bg-gray-400"
              type="button"
              @click="handleGenerate"
              :disabled="longUrl.length < 1"
            >
              Go
            </button>
          </div>
          <div v-if="generatedUrl.token" class="relative">
            <input
              class="block w-full p-4 pr-28 text-sm text-gray-900 border border-gray-300 rounded-md bg-gray-50 focus:ring-blue-500 focus:border-blue-500 dark:bg-gray-900 dark:border-gray-800 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500"
              v-model="completeGeneratedUrl"
              readonly
            />
            <button
              class="text-blue-700 w-20 absolute right-2.5 bottom-2.5 bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:outline-none focus:ring-blue-300 font-medium rounded-md text-sm px-4 py-2 dark:bg-gray-200 dark:hover:bg-gray-300 dark:focus:ring-blue-800"
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

export default {
  name: "IndexPage",
  data() {
    return {
      baseApiUrl: API_BASE_URL,
      longUrl: "",
      generatedUrl: "",
    };
  },

  mounted() {
    this.$refs.urlInput.focus();
  },

  computed: {
    completeGeneratedUrl() {
      return this.generatedUrl.token ? this.baseApiUrl + this.generatedUrl.token : "";
    },
  },
  methods: {
    async handleGenerate() {
      try {
        const data = await this.postUrl();
        this.generatedUrl = data;
      } catch (e) {
        const message = e.data.errors; //TODO
        alert(message);
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
