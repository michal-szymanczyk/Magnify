# Magnify  
  
In the project i tried to implement very basic "slices" architecture, which i was explaining during our call.  
To make things simple, i used only one project to implement that exercise. But in real live scenario, when microservice is more complex, then it is obvious to separate logical projects for data access, domain and API controllers.  
  
I tried to achieve as much as possible in the requested time so, not all unit tests were implemented and not all commands/queries features. It should show overall way of code style, structure, etc.  
  
# Cloud Question Answer  
  
I worked with Azure cloud services and i faced at least two major problems.  
First one is Service Bus, it's slow, sometimes (without any notice) it gets timeouts while making connections. In my opinion it's not stable enough (even if it's working fine for 6 months it does not guarantee it won't stop working). Next time, if i could choose i would go with RabbitMQ, it's has bigger audience so i think it should be more stable.  
  
Second issue is related to kubernetes clusters, from time to time very rarely, ingress controller randomly change routing of static IP addresses of pods. An outcome is that the pods cannot communicate to correct microservices through configured internal addresses. In that case only cluster version update solves the problem. I'm not dev ops so i'm not sure if that's some edge case related to cluster configuration or it's a matter of keeping versions up to date.
