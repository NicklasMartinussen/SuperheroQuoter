<template>
    <div class="quote-displayer-component">
        <h1>Superhero Quoter</h1>
        <p>Random superhero quotes every 10 seconds.</p>
        <br>

        <div v-if="loading">
            <h2>Loading... Please refresh once the ASP.NET backend has started.</h2>
        </div>

        <div v-if="randomQuote.author">
            <h2>"{{ randomQuote.text }}"</h2>
            <h3><i>- {{ randomQuote.author.name }}</i></h3>
        </div>
    </div>
</template>

<script lang="ts">
import { defineComponent } from 'vue';

interface Quote {
    id: number,
    text: string,
    author: { id: number, name: string }
}

export default defineComponent({
    data() {
        return {
            loading: false,
            quotes: [] as Quote[],
            randomQuote: [] as any,
        };
    },
    mounted: function () {
        window.setInterval(() => {
            this.setRandomQuote();
        }, 10000)
    },
    created() {
        this.fetchData();
    },
    methods: {
        async fetchData(): Promise<void> {
            this.quotes = [];
            this.loading = true;

            try {
                const response = await fetch('https://localhost:7285/api/Quotes');
                const data = await response.json();
                this.quotes = data;
                this.loading = false;
                this.setRandomQuote();
            } catch (error) {
                console.error(error);
            }
        },
        setRandomQuote() {
            if (this.quotes.length > 0) {
                var randomIndex = Math.floor(Math.random() * this.quotes.length);
                this.randomQuote = this.quotes[randomIndex];
            }
        }
    }
});
</script>

<style scoped>
.quote-displayer-component {
    text-align: center;
}
</style>