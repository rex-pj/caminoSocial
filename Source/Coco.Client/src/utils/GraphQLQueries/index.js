import gpl from "graphql-tag";

export const SIGN_UP = gpl`
  mutation Signup($criterias: SignupModelInput){
    signup(criterias: $criterias){
      isSucceed,
      errors {
        code,
        message
      }
    }
  }
`;

export const SIGNIN = gpl`
  mutation Signin($criterias: SigninModelInput!){
    signin(criterias: $criterias){
      userInfo {
        displayName,
        userIdentityId
      },
      authenticationToken
    }
  }
`;

export const UPDATE_USER_IDENTIFIER = gpl`
  mutation UpdateIdentifier($criterias: UserIdentifierUpdateDtoInput){
    updateIdentifier(criterias: $criterias){
      lastname
      firstname
      displayName
    }
  }
`;

export const GET_LOGGED_USER = gpl`
  query{
    loggedUser{
      lastname,
      firstname,
      email,
      displayName,
      userIdentityId,
      avatarUrl,
      coverPhotoUrl
    }
  }
`;

export const GET_USER_INFO = gpl`
  query($criterias: FindUserModelInput!){
    fullUserInfo(criterias: $criterias){
      canEdit
      email
      displayName
      userIdentityId
      address
      birthDate
      countryName
      description
      createdDate
      phoneNumber
      avatarUrl
      coverPhotoUrl
    }
  }
`;

export const GET_USER_IDENTIFY = gpl`
  query($criterias: FindUserModelInput!){
    fullUserInfo(criterias: $criterias){
      canEdit
      birthDate
      displayName
      email
      firstname
      lastname
      userIdentityId
    }
  }
`;

export const GET_FULL_USER_INFO = gpl`
  query($criterias: FindUserModelInput!){
    fullUserInfo(criterias: $criterias){
      canEdit
      birthDate
      displayName
      email
      firstname
      lastname
      createdDate
      description
      address
      phoneNumber
      genderLabel
      genderId
      countryName
      countryId
      statusLabel
      userIdentityId
      coverPhotoUrl
      genderSelections {
        id
        text
      }
      countrySelections {
        id
        name
      }
    }
  }
`;

export const UPDATE_USER_INFO_PER_ITEM = gpl`
  mutation UpdateUserInfoItem($criterias: UpdatePerItemModelInput!){
    updateUserInfoItem(criterias: $criterias){
      value
      propertyName
    }
  }
`;

export const UPDATE_USER_AVATAR = gpl`
  mutation UpdateAvatar($criterias: UserPhotoUpdateDtoInput!){
    updateAvatar(criterias: $criterias){
      isSucceed,
      errors {
        code
        message
      }
    }
  }
`;

export const DELETE_USER_AVATAR = gpl`
  mutation DeleteAvatar($criterias: PhotoDeleteModelInput!){
    deleteAvatar(criterias: $criterias){
      isSucceed,
      errors {
        code
        message
      }
    }
  }
`;

export const UPDATE_USER_COVER = gpl`
  mutation UpdateCover($criterias: UserPhotoUpdateDtoInput!){
    updateCover(criterias: $criterias){
      isSucceed,
      errors {
        code
        message
      }
    }
  }
`;

export const DELETE_USER_COVER = gpl`
  mutation DeleteCover($criterias: PhotoDeleteModelInput!){
    deleteCover(criterias: $criterias){
      isSucceed,
      errors {
        code
        message
      },
      result
    }
  }
`;

export const UPDATE_USER_PASSWORD = gpl`
  mutation UpdatePassword($criterias: UserPasswordUpdateDtoInput!){
    updatePassword(criterias: $criterias){
      authenticationToken
    }
  }
`;

export const FORGOT_PASSWORD = gpl`
  mutation ForgotPassword($criterias: ForgotPasswordModelInput!){
    forgotPassword(criterias: $criterias){
      isSucceed,
      errors {
        code,
        message
      }
    }
  }
`;

export const SIGNOUT = gpl`
query signout{
    signout {
      isSucceed,
      errors {
        code,
        message
      }
    }
  }
`;

export const ACTIVE = gpl`
query ($criterias: ActiveUserModelInput!){
  active(criterias: $criterias) {
      isSucceed,
      errors {
        code,
        message
      }
    }
  }
`;

export const RESET_PASSWORD = gpl`
mutation ($criterias: ResetPasswordModelInput!){
  resetPassword(criterias: $criterias) {
      isSucceed,
      errors {
        code,
        message
      }
    }
  }
`;
