<template>
  <div class="mx-auto mt-16 max-w-4xl">
    <h2 class="text-3xl font-bold text-center dark:text-white mb-8">🔗 Your Shortened Links</h2>

    <div v-if="loading" class="text-center text-gray-500 dark:text-gray-400">
      Loading your links...
    </div>

    <div v-if="error" class="text-center text-red-600 dark:text-red-400">
      {{ error }}
    </div>

    <div v-if="!loading && links.length === 0" class="text-center text-gray-500 dark:text-gray-400">
      You don't have any links yet.
    </div>

    <div v-if="!loading && links.length" class="overflow-x-auto w-full">
      <LinksTable :links="links" @copy="copyShortUrl" />
    </div>
  </div>
</template>

<script setup>
import { ref, onMounted } from "vue";
import { useHead } from "#app";
import { API_BASE_URL } from "@/constants";

const links = ref([]);
const loading = ref(true);
const error = ref("");

useHead({
  title: "Your Links | ShortURLs",
});

const shortUrl = (token) => `${API_BASE_URL}${token}`;

const copyShortUrl = async (token) => {
  try {
    await navigator.clipboard.writeText(shortUrl(token));
    alert("Short URL copied!");
  } catch {
    alert("Failed to copy link.");
  }
};

const fetchLinks = async () => {
  loading.value = true;
  try {
    const response = await useAuthFetch(`${API_BASE_URL}links`);

    if (response.error?.value?.statusCode === 401) {
      localStorage.removeItem("token");
      router.push("/login");

      return;
    }

    links.value = response.data?.value?.items || [];
  } catch (err) {
    error.value = err?.data?.message || "Failed to load links.";
  } finally {
    loading.value = false;
  }
};

onMounted(() => {
  fetchLinks();
});
</script>
