<template>
    <div class="add-twit-form">
        <div>
            <div class="twit-input">
                <label for="twit-text">Enter twit text: </label>
                <input type="text" name="twit-text" id="twit-text" required>
            </div>
            <div class="submit-btn">
                <input type="submit" value="Submit" @click="sendRequest()">
            </div>
        </div>
    </div>
</template>
<script>
import { computed, inject } from "vue";

export default {
    name: "AddTwitComponent",
    props: {},
    setup() {
        const store = inject("store");

        // computed
        const loggedUser = computed(() => store.users.state.loggedUser);

        // functions
        const submitTwit = (twitData) => store.twits.actions.addTwit(twitData);

        return { 
            loggedUser,
            submitTwit
        }
        
    },
    methods: {
        async sendRequest() {
            const text = document.getElementById("twit-text").value;
            const twitData = {
                "AuthorId": this.loggedUser,
                "Text": text
            };
            await this.submitTwit(twitData);
        }
    }
}
</script>
<style lang="scss">
[class$="-form"] {
    height: max-content;
}
</style>