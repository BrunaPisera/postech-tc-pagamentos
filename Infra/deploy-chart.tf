provider "helm" {
  kubernetes {
    host                   = data.aws_eks_cluster.tc_eks_cluster.endpoint
    cluster_ca_certificate = base64decode(data.aws_eks_cluster.tc_eks_cluster.certificate_authority[0].data)
    exec {
      api_version = "client.authentication.k8s.io/v1beta1"
      args        = ["eks", "get-token", "--cluster-name", "eks_lanchonete-do-bairro"]
      command     = "aws"
    }
  }
}

resource "helm_release" "pagamentos" {
  name             = "pagamentos"
  namespace        = "dev"
  create_namespace = true
  chart            = "../helm/pagamentos-chart"

  values = [
    file("../helm/pagamentos-chart/values.yaml"),
    file("../helm/pagamentos-chart/values-dev.yaml")
  ]

  set {
    name  = "configmap.data.DB_PASSWORD"
    value = var.dbpassword
  }

  set {
    name  = "rabbitmq.password"
    value = var.brokerpassword
  }

  set {
    name  = "mpConfigMap.data.ACCESS_KEY"
    value = var.apikeymp
  }
}