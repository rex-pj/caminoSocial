import React, { useState, useCallback } from "react";
import styled from "styled-components";
import { SecondaryTextbox } from "../../atoms/Textboxes";
import { PanelBody, PanelFooter } from "../../atoms/Panels";
import { LabelNormal } from "../../atoms/Labels";
import { ButtonPrimary } from "../../atoms/Buttons/Buttons";
import ForgotPasswordNavigation from "../Navigation/ForgotPasswordNavigation";
import AuthBanner from "../Banner/AuthBanner";
import forgotPasswordModel from "../../../models/forgotPasswordModel";
import { checkValidity } from "../../../utils/Validity";
import { PrimaryNotice } from "../../atoms/Notices/AlertNotice";
import { withRouter } from "react-router-dom";

const Textbox = styled(SecondaryTextbox)`
  border-radius: ${(p) => p.theme.size.normal};
  border: 1px solid ${(p) => p.theme.color.secondaryBg};
  background-color: ${(p) => p.theme.rgbaColor.lighter};
  width: 100%;
  color: ${(p) => p.theme.color.darkText};
  padding: ${(p) => p.theme.size.tiny};

  ::placeholder {
    color: ${(p) => p.theme.color.neutralText};
    font-size: ${(p) => p.theme.fontSize.small};
  }

  :focus {
    background-color: ${(p) => p.theme.color.moreDark};
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

const SubmitButton = styled(ButtonPrimary)`
  font-size: ${(p) => p.theme.fontSize.small};
  border: 1px solid ${(p) => p.theme.color.secondaryBg};

  :hover {
    color: ${(p) => p.theme.color.neutralText};
  }

  :disabled {
    background-color: ${(p) => p.theme.color.secondaryBg};
    color: ${(p) => p.theme.color.neutralText};
    cursor: auto;
  }
`;

export default withRouter((props) => {
  const [, updateState] = useState();
  const forceUpdate = useCallback(() => updateState({}), []);
  const [isFormEnabled, setFormEnabled] = useState(false);
  const [isSubmitted, setSubmitted] = useState(false);
  let formData = forgotPasswordModel;

  const handleInputBlur = (evt) => {
    const { name } = evt.target;
    if (!formData[name].isValid) {
      evt.target.classList.add("invalid");
    } else {
      evt.target.classList.remove("invalid");
    }
  };

  const handleInputChange = (evt) => {
    if (isSubmitted) {
      setSubmitted(false);
    }

    formData = formData || {};
    const { name, value } = evt.target;

    const prevValid = formData[name].isValid;
    // Validate when input
    formData[name].isValid = checkValidity(formData, value, name);
    formData[name].value = value;

    if (prevValid !== formData[name].isValid) {
      forceUpdate();
    }

    if (formData[name].isValid && !isFormEnabled) {
      setFormEnabled(true);
    }
  };

  const onUpdate = async (e) => {
    e.preventDefault();

    if (isFormEnabled) {
      setFormEnabled(false);
    }

    if (!isSubmitted) {
      setSubmitted(true);
    }

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
      const requestData = {};
      for (const formIdentifier in formData) {
        requestData[formIdentifier] = formData[formIdentifier].value;
      }

      await props
        .onForgotPassword(requestData)
        .then(() => {
          setFormEnabled(true);
        })
        .catch(() => {
          setFormEnabled(true);
        });
    }
  };

  return (
    <form onSubmit={(e) => onUpdate(e)} method="POST">
      <div className="row g-0">
        <div className="col col-12 col-sm-7">
          <AuthBanner icon="unlock-alt" title="Phục hồi mật khẩu" />
        </div>
        <div className="col col-12 col-sm-5">
          <ForgotPasswordNavigation />
          <PanelBody>
            <FormRow>
              {isSubmitted ? (
                <PrimaryNotice>
                  We will send an activation e-mail to you, please check your
                  e-mail, if you couldn't found may it is in spam folder
                </PrimaryNotice>
              ) : null}
            </FormRow>
            <FormRow>
              <Label>E-mail</Label>
              <Textbox
                placeholder="Please input your e-mail"
                type="email"
                name="email"
                autoComplete="off"
                onChange={(e) => handleInputChange(e)}
                onBlur={(e) => handleInputBlur(e)}
              />
            </FormRow>
            <FormFooter>
              <SubmitButton disabled={!isFormEnabled} type="submit">
                Send
              </SubmitButton>
            </FormFooter>
          </PanelBody>
        </div>
      </div>
    </form>
  );
});
