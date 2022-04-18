# Selecting a license

To select the most suitable license we had to do some investigation. Based on our research we have identified two different types of dependencies:

- Direct dependencies
- Transitive dependencies

Direct dependencies are libraries our code directly interacts with. Transitive dependencies are dependencies of dependencies. One has to identify all the applicable licenses and abide by all the obligations. [[https://www.synopsys.com/blogs/software-security/open-source-license-compliance-dependencies/](https://www.synopsys.com/blogs/software-security/open-source-license-compliance-dependencies/)] Transitive dependencies are most of the time low risk, and most of the attention should be directed towards direct dependencies.

We have used tools to identify both the front- and backend’s dependencies and we have collected them in their respective json files. Regarding the frontend, only one dependency seemed alarming at first glance, which was the node-forge package [[https://www.npmjs.com/package/node-forge](https://www.npmjs.com/package/node-forge)]. Its license is BSD-3-Clause OR GPL-2.0. SonarAnalyzer.CSharp [[https://www.sonarlint.org/visualstudio/](https://www.sonarlint.org/visualstudio/)] was concerning on the backend side as it has LGPL-3.0-only license. Both of these discoveries contained some form of protective license, hence the caution on our side. However, the source code does not contain derivative work of the library, it is designed to work with it, so it falls outside of the scope of that license. 

Based on our research we have concluded that both  the front- and backend projects will be licensed under GPL v3

By using GPL v3 our software is free software, and it stays that way:

- the freedom to use the software for any purpose,
- the freedom to change the software to suit your needs,
- the freedom to share the software with your friends and neighbors, and
- the freedom to share the changes you make.