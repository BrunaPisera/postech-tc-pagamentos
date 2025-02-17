variable "defaultRegion" {
  default = "us-east-1"
}

variable "dbpassword" {
  description = "Secret passed from GitHub Actions"
  type        = string
}

variable "brokerpassword" {
  description = "Secret passed from GitHub Actions"
  type        = string
}

variable "apikeymp" {
  description = "Secret passed from GitHub Actions"
  type        = string
}