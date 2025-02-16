terraform {
  backend "s3" {
    bucket = "tc-tf-pagamentos"
    key    = "backend/terraform.tfstate"
    region = "us-east-1"
  }
}