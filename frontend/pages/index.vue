<template>
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

        <div v-if="generatedToken.length" class="relative">
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
</template>

<script setup>
import { onMounted, ref, computed } from "vue";
import { API_BASE_URL } from "@/constants";
import { useAuthFetch } from "~/composables/useAuthFetch";
import { urlRegex } from "~/utils/regex-utils";

const longUrl = ref("");
const generatedToken = ref("");
const loading = ref(false);
const urlInput = ref(null);

const invalidUrl = computed(() => !urlRegex.test(longUrl.value));

const completeGeneratedUrl = computed(() =>
  generatedToken.value ? `${API_BASE_URL}${generatedToken.value}` : ""
);

const handleGenerate = async () => {
  if (invalidUrl.value) return;

  loading.value = true;
  try {
    // TODO save with a name
    const { data } = await useAuthFetch(`${API_BASE_URL}links`, {
      method: "POST",
      body: { url: longUrl.value },
    });

    generatedToken.value = data.value.shortCode;
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
  urlInput.value?.focus();
});
</script>
