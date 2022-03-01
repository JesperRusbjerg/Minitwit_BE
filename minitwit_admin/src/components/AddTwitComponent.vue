<template>
    <div class="form">
        <div class="form-wrapper">
            <div class="twit-input">
                <p>Create new twit:</p>
                <textarea name="twit-text" placeholder="Twit some more 🦜" id="twit-text"/>
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
@import "../genericStyles.scss"; 

.form {
    width: 70%;
    height: fit-content;
    margin: auto 15%;

    .form-wrapper {
        > * {
            padding: 10px 0;
        }

        textarea { 
            @include roundedBorders;
            max-height: 5rem;
            max-width: 80%;
        }
    }

}
</style>