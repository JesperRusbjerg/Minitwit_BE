## Version Control - Git + github.com

In order to share our work and keep track of its changes, it has been decided to use Git as the version control system (VCS). Git is defined as a distributed VCS. In comparison to centralized VCS, the local version directly reflects the entire structure together with the history of the version stored in the cloud. Hence, it provides a convenient fallback in case a cloud-hosted source becomes unavailable. It also allows for collaboration as well as distribution of workload upon the same project, while referring to the same project version. 

As for the repository hosting service, github.com was selected. The reason for that is its compatibility with 3 parties, ability to authenticate with a Github account on many other platforms and integrability with various local IDEs. Moreover, the system of PRs, issue creation, or whole project management is suitably accessible from one place.


## Cloud hosting - Digital Ocean

Among many available cloud hosting providers, Digital Ocean (DO) was chosen as the primary one for our project. This decision was impacted by, firstly, the constraint of choosing an IaaS provider and secondly an optimized, non-overcomplex solution for a small project. DO provides a set of products that meets our criteria just right without the need of configuring endless settings as it was a large, enterprise project. Additionally, it provides reasonably good and concise documentation together with free developer support if needed. Moreover, as opposed to some of its competitors, it presents a fixed price per product, which reduces the possibility of ending up paying ‘hidden costs’.

## Frontend - Vue3 + JavaScript + webpack

As for the development of the project's UI, it has been decided to use Vue3. Vue.js is a JavaScript framework based on the hierarchy of embedded Single-File Components (SFC), which are manipulated through an API (Options or Composition API). The result is declarative rendering based on JavaScript state as well as reactivity upon direct DOM changes. It comes together with functionalities building on top of HTML as well as a library of subprojects like DevTools, Router, or Event Bus that makes provides handlers for otherwise complicated logistics in a concise form. Vue3 has been opted for as it became a Vue default version as of 7.02.2022. Moreover, it proves to perform faster upon changes and maintain projects in a lighter format than the previous Vue version.

Aside from utilizing, render-connected languages like CSS (with SCSS) and HTML, JavaScript is used for the logical execution of actions. It provides high suitability for the development of reactive and dynamic web applications due to its JIT compilation supported by most nowadays browsers. Moreover, it comes with an extensive active community, hence handful of interfaces and libraries that can enrich the web content is available.

For the web application to run seamlessly on the server, a webpack module bundler has been opted for. The tool allows for fast compilation of JavaScript-based project into a dependency graph and generates singular files that run the entire application. Webpack provides an optimized way of serving content through the creation of necessary static assets.

## Monitoring - Prometheus + Grafana

To provide a monitoring system for our project, a Prometheus monitoring solution is applied. It provides an entire monitoring and trending system based on pulling rather than pushing - it pulls/scrapes the data from available endpoints, which creates a time-based-alike database of metrics. Furthermore, Prometheus has its own querying language PromQL that allows for prompt time series data selection. This is then utilized by Grafana - a visualization tool for gathered metrics. Grafana performs upon the pulling strategy as well with continuous updates, which can be then displayed with a number of pre-created dashboard templates and graphs.