
#  _                _
# | | ___  __ _  __| | ___ _ __
# | |/ _ \/ _` |/ _` |/ _ \ '__|
# | |  __/ (_| | (_| |  __/ |
# |_|\___|\__,_|\__,_|\___|_|

# create cloud vm
resource "digitalocean_droplet" "minitwit-swarm-leader" {
  image = "docker-18-04"
  name = "minitwit-swarm-leader"
  region = var.region
  size = "s-1vcpu-1gb"
  # add public ssh key so we can access the machine
  ssh_keys = [digitalocean_ssh_key.minitwit.fingerprint]

  # specify a ssh connection
  connection {
    user = "root"
    host = self.ipv4_address
    type = "ssh"
    private_key = file(var.pvt_key)
    timeout = "2m"
  }

  provisioner "file" {
    source = "stack/minitwit_stack.yml"
    destination = "/root/minitwit_stack.yml"
  }

 provisioner "file" {
   source = "temp/prometheus.yml"
   destination = "/root/prometheus.yml"

}


  provisioner "remote-exec" {
    inline = [
     # allow ports for docker swarm
   #   "ufw allow 2377/tcp",
      #"ufw allow 7946",
      #"ufw allow 4789/udp",
      # ports for apps
#      "ufw allow 8081",
 #     "ufw allow 8080",
  #    "ufw allow 9091",
     "sudo ufw disable",
     "mkdir gfdata",
     "docker swarm init --advertise-addr ${self.ipv4_address}"
    ]
  }

  # save the worker join token
  provisioner "local-exec" {
    command = "ssh -o 'StrictHostKeyChecking no' root@${self.ipv4_address} -i ssh_key/terraform 'docker swarm join-token worker -q' > temp/worker_token"
  }

  # save the manager join token
#  provisioner "local-exec" {
 #   command = "ssh -o 'StrictHostKeyChecking no' root@${self.ipv4_address} -i ssh_key/terraform 'docker swarm join-token manager -q' > temp/manager_token"
 # }
}

#                     _
# __      _____  _ __| | _____ _ __
# \ \ /\ / / _ \| '__| |/ / _ \ '__|
#  \ V  V / (_) | |  |   <  __/ |
#   \_/\_/ \___/|_|  |_|\_\___|_|
#
# create cloud vm
resource "digitalocean_droplet" "minitwit-swarm-worker" {
  # create workers after the leader
  depends_on = [digitalocean_droplet.minitwit-swarm-leader]

  # number of vms to create
  count = 2

  image = "docker-18-04"
  name = "minitwit-swarm-worker-${count.index}"
  region = var.region
  size = "s-1vcpu-1gb"
  # add public ssh key so we can access the machine
  ssh_keys = [digitalocean_ssh_key.minitwit.fingerprint]

  # specify a ssh connection
  connection {
    user = "root"
    host = self.ipv4_address
    type = "ssh"
    private_key = file(var.pvt_key)
    timeout = "2m"
  }

  provisioner "file" {
    source = "temp/worker_token"
    destination = "/root/worker_token"
  }

 provisioner "file" {
   source = "temp/prometheus.yml"
   destination = "/root/prometheus.yml"

}


  provisioner "remote-exec" {
    inline = [
          # allow ports for docker swarm
      #"ufw allow 2377/tcp",
      #"ufw allow 7946",
      #"ufw allow 4789/udp",
      # ports for apps
      #"ufw allow 8081",
      #"ufw allow 8080",
      #"ufw allow 9091",
      "sudo ufw disable",
      "mkdir gfdata",
      # join swarm cluster as workers
      "docker swarm join --token $(cat worker_token) ${digitalocean_droplet.minitwit-swarm-leader.ipv4_address}"
    ]
  }
}

output "minitwit-swarm-leader-ip-address" {
  value = digitalocean_droplet.minitwit-swarm-leader.ipv4_address
}


output "minitwit-swarm-worker-ip-address" {
  value = digitalocean_droplet.minitwit-swarm-worker.*.ipv4_address
}
