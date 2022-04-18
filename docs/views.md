# Architectural Views

## Module View

This view gives a high level overview of the structure of the system.

The second module view is a zoomed in, but still high level view of the MiniTwit Backend. It shows the relationships between different layers in the architecture.

![Module View](/../screenshots/architecturalViews/minitwit_module_view_v2.png?raw=true "Module View")\
![MiniTwit Backend](/../screenshots/architecturalViews/minitwit_module_view_v2_higher.png?raw=true "MiniTwit Backend")

## Component-and-Connector View

This view serves to showcase communication between components. These contain communication links, protocols, information flows and access to storage. [Software Architecture in Practice 4th Edition]

The flow from the browser all the way to our cluster is visible in this view. It also showcases exposed ports and communication protocols.

![C&C View](/../screenshots/architecturalViews/minitwit_cc_view_v2.png?raw=true "C&C View")

## Allocation View

This view presents how our software units are mapped to environments in which they are operating or executing.

The executables of the frontend application are placed in a containerized execution environment, which is wrapped by a Linux virtual machine in a DigitalOcean cloud environment. The contents of the backend deployment are part of a Docker Swarm cluster and part of worker nodes, which are orchestrated by manager nodes. Each worker node contains a DigitalOcean Droplet with a Linux virtual machine that contains containerized applications, such as the MiniTwit Backend, a MariaDB database, Prometheus and Grafana.

![Allocation View](/../screenshots/architecturalViews/minitwit_allocation_view_v2.png?raw=true "Allocation View")