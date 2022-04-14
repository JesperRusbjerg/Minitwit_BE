# Service Level Agreement

The deadly algorithm SLA is a set of policies that obligates certain standards of our system to 
be upheld, and in case it cannot we will reimburse the wronged party. 

This SLA includes the minitwit api and minitwit frontend.

## Monthly Uptime Percentage
Less than 95% uptime  -> 10 credits of reimbursement 

Less than 99% but more than 95% uptime -> 5 credits of reimbursement

Less than 99.99% but more than 99% uptime -> 2 credits of reimbursement

We calculate the uptime by (Number of successful request / Total number of request )
We count all failed request within the same 30 second time period as a single 
failed request

Back-off requirement: For each failed request, wait 30 seconds before sending the next one.

## SLA Exclusions
We only count downtime on our own services as part of the SLA. We do not take 
responsibility if third party services are down. This includes but are not limited to 
hosting, data retrieval, payment etc

The SLA does not adhere to factors outside of group M's control, such as: Errors caused by the browsers or wrongful use of the API's endpoints, invalid request headers, unauthorized users.

## Redeem Credits
In order to obtain credits in the case of the SLA not being upheld, the customer must take one of the following actions by submitting an issue:

1. Within 24 hours of experiencing the SLA breech, create a github issue on: https://github.com/DeadlyDevops/minitwit_BE.
2. Find a group M member at ITU and submit the issue in person

### Issue template

Name of company:

What did you observe?

Provide documentation

## Credits spending
Credits can be spent as follows:

1 Credit: 1 hug from a person of group M of your choosing

10 Credits: A group hug with group M

