## Risk identification

**Identify assets:**

* Programs:
    * Back-end web application
    * Front-end web application
    * Prometheus
    * Grafana
    * DataDog log agent
* Servers / Environments:
    * Travis CI pipeline
    * Backend Digital Ocean droplet
    * Frontend Digital Ocean droplet
    * DataDog PaaS

**Threat sources:**

* Hacker / Cybercriminal
* Insider
* Competitors (other groups)
* Environment

Both hackers/cybercriminals, insiders, and competitors are related to as attackers in the subsequent sections.


**Threats with risk scenarios:**

For our identifications of threats, we are using OWASP's top 10 vulnerabilities for 2021 as well as other sources and personal experience.

1. Broken access control:
    - An attacker forces the frontend application to target the user profile page URL without authorization.
    - An attacker accesses Digital Ocean droplets or our Grafana dashboard.
2. Cryptographic Failures:
    - An attacker eavesdrops on the communication between the frontend and the backend.
    - An attacker de-hashes users' passwords stored in the database (attacker has access to the database).
3. Injection attacks:
    - An attacker tries to perform SQL injection on the backend.
    - An attacker tries to perform XSS on the frontend.
4. Insecure Design:
    - An attacker tries to utilize the flaw in the application design to perform an attack.
5. Security Misconfiguration
    - An attacker uses unnecessarily opened ports to attack the system.
    - An attacker utilizes error stack traces to gain information about the system.
6. Vulnerable and Outdated Components:
    - An attacker uses the common vulnerability of a system component.
    - An attacker uses a common vulnerability of application libraries.
7. Identification and Authentication Failures
    - An attacker performs a brute force attack to guess the user's password.
    - An attacker tries to utilize default credentials to log in to a system component.
8. Software and Data Integrity Failures
    - An attacker does a man-in-the-middle attack and modifies user requests on the fly.
9. Security Logging and Monitoring Failures
    - An attacker performs a successful system attack undetected.
    - An attacker gains access to DataDog and finds sensitive user information in the logs.
10. Server-Side Request Forgery
    - An attacker tries the Remote Code Execution on the server behind the firewall.
11. Denial-of-service
    - An attacker performs a denial-of-service attack on the system.
12. Social engineering
    - An attacker uses social engineering techniques to persuade the development team to gain access to the system.
13. Lack of data recovery plans
    - In the event of a successful attack or system corruption, a system doesn’t have backups.
14. Exposed application secrets
    - An attacker accesses the group’s public repo and finds secrets in plaintext.


##


## Risk analysis


<table>
  <tr>
   <td>Source</td>
   <td>Likelihood</td>
   <td>Impact</td>
   <td>Risk</td>
   <td>Actions</td>
  </tr>
  <tr>
   <td>1A</td>
   <td>L</td>
   <td>M</td>
   <td>L</td>
   <td>-</td>
  </tr>
  <tr>
   <td>1B</td>
   <td>L</td>
   <td>H</td>
   <td>M</td>
   <td>-</td>
  </tr>
  <tr>
   <td>2A</td>
   <td>H</td>
   <td>M/H</td>
   <td>H</td>
   <td>TLS could be implemented for the frontend.
<p>
Another option is to use VPC Network (a private network within DO that attacker doesn’t have access to) for connections between BE and FE.</td>
  </tr>
  <tr>
   <td>2B</td>
   <td>M</td>
   <td>H</td>
   <td>H</td>
   <td>Investigate the possibility of storing random salt alongside passwords in the database.</td>
  </tr>
  <tr>
   <td>3A</td>
   <td>L (1)</td>
   <td>H</td>
   <td>M</td>
   <td>-</td>
  </tr>
  <tr>
   <td>3B</td>
   <td>L (3)</td>
   <td>M</td>
   <td>L</td>
   <td>-</td>
  </tr>
  <tr>
   <td>4A</td>
   <td>L</td>
   <td>M</td>
   <td>L</td>
   <td>-</td>
  </tr>
  <tr>
   <td>5A</td>
   <td>L</td>
   <td>H</td>
   <td>M</td>
   <td>Go through the system and close unnecessarily opened ports.</td>
  </tr>
  <tr>
   <td>5B</td>
   <td>L</td>
   <td>M</td>
   <td>L</td>
   <td>Verify that the stack trace is not attached to any BE responses, nor visible in the FE console.</td>
  </tr>
  <tr>
   <td>6A</td>
   <td>L (4)</td>
   <td>M/H</td>
   <td>L</td>
   <td>-</td>
  </tr>
  <tr>
   <td>6B</td>
   <td>L (4)</td>
   <td>M/H</td>
   <td>L</td>
   <td>-</td>
  </tr>
  <tr>
   <td>7A</td>
   <td>M</td>
   <td>M</td>
   <td>M</td>
   <td>Investigate how to limit failed login attempts.</td>
  </tr>
  <tr>
   <td>7B</td>
   <td>L</td>
   <td>M</td>
   <td>L</td>
   <td>-</td>
  </tr>
  <tr>
   <td>8A</td>
   <td>H</td>
   <td>M/H</td>
   <td>H</td>
   <td>Same as 2A.</td>
  </tr>
  <tr>
   <td>9A</td>
   <td>L</td>
   <td>L</td>
   <td>L</td>
   <td>-</td>
  </tr>
  <tr>
   <td>9B</td>
   <td>L</td>
   <td>L</td>
   <td>L</td>
   <td>-</td>
  </tr>
  <tr>
   <td>10A</td>
   <td>L</td>
   <td>H</td>
   <td>M</td>
   <td>Limit outbound ports on the firewall.</td>
  </tr>
  <tr>
   <td>11A</td>
   <td>L/M</td>
   <td>M/H</td>
   <td>M</td>
   <td>Limit inbound ports on the firewall.</td>
  </tr>
  <tr>
   <td>12A</td>
   <td>L (2)</td>
   <td>H</td>
   <td>M</td>
   <td>-</td>
  </tr>
  <tr>
   <td>13A</td>
   <td>M</td>
   <td>H</td>
   <td>H</td>
   <td>Investigate how to do database backups.</td>
  </tr>
  <tr>
   <td>14A</td>
   <td>H</td>
   <td>H</td>
   <td>H</td>
   <td>Hide remaining secrets and delete unused files.</td>
  </tr>
</table>


1. Entity Framework with LINQ queries are not susceptible to traditional SQL injection attacks.
2. The group can only be persuaded with a sufficient amount of beer :)
3. Vue automatically escapes HTML content ([https://v2.vuejs.org/v2/guide/security.html](https://v2.vuejs.org/v2/guide/security.html)).
4. Pen test was performed with tools like Metasploit.