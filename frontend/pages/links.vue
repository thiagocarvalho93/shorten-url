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

    <div v-if="!loading && links.length" class="overflow-x-auto">
      <table class="min-w-full border border-gray-300 dark:border-gray-600 rounded-md">
        <thead class="bg-gray-100 dark:bg-gray-700 text-gray-900 dark:text-white">
          <tr>
            <th class="p-4 text-left">Original URL</th>
            <th class="p-4 text-left">Short URL</th>
            <th class="p-4">Actions</th>
          </tr>
        </thead>
        <tbody class="bg-white dark:bg-gray-800 text-gray-700 dark:text-gray-200">
          <tr
            v-for="link in links"
            :key="link.token"
            class="border-t border-gray-200 dark:border-gray-600"
          >
            <td class="p-4 break-all text-ellipsis">{{ link.originalUrl }}</td>
            <td class="p-4 break-all">
              <a
                :href="shortUrl(link.shortCode)"
                target="_blank"
                class="text-blue-600 dark:text-blue-400 hover:underline"
              >
                {{ shortUrl(link.shortCode) }}
              </a>
            </td>
            <td class="p-4 text-center">
              <button
                @click="copyShortUrl(link.shortCode)"
                class="px-3 py-1 text-sm bg-blue-600 hover:bg-blue-700 text-white rounded-md"
              >
                Copy
              </button>
            </td>
          </tr>
        </tbody>
      </table>
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
    const { data } = await useAuthFetch(`${API_BASE_URL}links`);

    console.log(data.value.items);

    links.value = data?.value?.items || [];
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
