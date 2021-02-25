import React, { useState, useCallback } from "react";
import styled from "styled-components";
import { Link } from "react-router-dom";
import { SecondaryTextbox } from "../../atoms/Textboxes";
import { PanelBody, PanelFooter } from "../../atoms/Panels";
import { LabelNormal } from "../../atoms/Labels";
import { ButtonLight } from "../../atoms/Buttons/Buttons";
import AuthNavigation from "../Navigation/AuthNavigation";
import AuthBanner from "../Banner/AuthBanner";
import loginModel from "../../../models/loginModel";
import { checkValidity } from "../../../utils/Validity";

const Textbox = styled(SecondaryTextbox)`
  border-radius: ${(p) => p.theme.size.normal};
  border: 1px solid ${(p) => p.theme.color.neutralBg};
  background-color: ${(p) => p.theme.color.lightBg};
  width: 100%;
  color: ${(p) => p.theme.color.darkText};
  padding: ${(p) => p.theme.size.tiny};

  ::placeholder {
    color: ${(p) => p.theme.color.neutralText};
    font-size: ${(p) => p.theme.fontSize.small};
  }

  :focus {
    background-color: ${(p) => p.theme.color.neutralBg};
    color: ${(p) => p.theme.color.whiteText};
  }

  &.invalid {
    border: 1px solid ${(p) => p.theme.color.secondaryDangerBg};
  }
`;

const FormFooter = styled(PanelFooter)`
  text-align: center;
`;

const Label = styled(LabelNormal)`
  margin-left: ${(p) => p.theme.size.tiny};
  margin-bottom: 0;
  font-size: ${(p) => p.theme.fontSize.small};
  font-weight: 600;
`;

const FormRow = styled.div`
  margin-bottom: ${(p) => p.theme.size.tiny};
`;

const SubmitButton = styled(ButtonLight)`
  font-size: ${(p) => p.theme.fontSize.small};
  border: 1px solid ${(p) => p.theme.color.primaryBg};

  :hover {
    background-color: ${(p) => p.theme.color.lightBg};
    color: ${(p) => p.theme.color.neutralText};
  }

  :disabled {
    background-color: ${(p) => p.theme.color.neutralBg};
    color: ${(p) => p.theme.color.primaryText};
    cursor: auto;
  }
`;

const ForgotPasswordRow = styled(FormRow)`
  margin-top: ${(p) => p.theme.size.exTiny};
  font-size: ${(p) => p.theme.fontSize.small};
  a {
    color: ${(p) => p.theme.color.neutralText};
  }
  text-align: center;
`;

export default (props) => {
  const [, updateState] = useState();
  const forceUpdate = useCallback(() => updateState({}), []);
  let formData = loginModel;

  const handleInputBlur = (evt) => {
    const { name } = evt.target;
    if (!formData[name].isValid) {
      evt.target.classList.add("invalid");
    } else {
      evt.target.classList.remove("invalid");
    }
  };

  const handleInputChange = (evt) => {
    formData = formData || {};
    const { name, value } = evt.target;

    // Validate when input
    formData[name].isValid = checkValidity(formData, value, name);
    formData[name].value = value;

    forceUpdate();
  };

  const checkIsFormValid = () => {
    let isFormValid = false;
    for (let formIdentifier in formData) {
      isFormValid = formData[formIdentifier].isValid;
      if (!isFormValid) {
        break;
      }
    }

    return isFormValid;
  };

  const onlogin = (e) => {
    e.preventDefault();

    let isFormValid = true;
    for (let formIdentifier in formData) {
      isFormValid = formData[formIdentifier].isValid && isFormValid;

      if (!isFormValid) {
        props.showValidationError(
          "Something went wrong with your input",
          "Something went wrong with your information, please check and input again"
        );
      }
    }

    if (!!isFormValid) {
      const loginData = {};
      for (const formIdentifier in formData) {
        loginData[formIdentifier] = formData[formIdentifier].value;
      }

      props.onlogin(loginData);
    }
  };

  const isFormValid = checkIsFormValid();

  return (
    <form onSubmit={(e) => onlogin(e)} method="POST">
      <div className="row g-0">
        <div className="col col-12 col-sm-7">
          <AuthBanner
            imageUrl={`${process.env.PUBLIC_URL}/images/logo.png`}
            title="Login"
            instruction="Join with us to connect with many other farmers"
          />
        </div>
        <div className="col col-12 col-sm-5">
          <AuthNavigation />
          <PanelBody>
            <FormRow>
              <Label>E-mail</Label>
              <Textbox
                placeholder="Please input your e-mail"
                type="email"
                name="username"
                onChange={(e) => handleInputChange(e)}
                onBlur={(e) => handleInputBlur(e)}
              />
            </FormRow>
            <FormRow>
              <Label>Password</Label>
              <Textbox
                placeholder="Password"
                type="password"
                name="password"
                onChange={(e) => handleInputChange(e)}
                onBlur={(e) => handleInputBlur(e)}
              />
            </FormRow>
            <FormFooter>
              <SubmitButton
                disabled={!props.isFormEnabled || !isFormValid}
                type="submit"
              >
                Login
              </SubmitButton>
            </FormFooter>
            <ForgotPasswordRow>
              <Link to="/auth/forgot-password">Forgot your password?</Link>
            </ForgotPasswordRow>
          </PanelBody>
        </div>
      </div>
    </form>
  );
};
