data "aws_iam_role" "labrole" {
  name = "LabRole"
}

data "aws_eks_cluster" "tc_eks_cluster" {
  name = "eks_lanchonete-do-bairro"
}