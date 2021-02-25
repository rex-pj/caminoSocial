import React, { Fragment, useState, useCallback } from "react";
import styled from "styled-components";
import { FontAwesomeIcon } from "@fortawesome/react-fontawesome";
import { PanelBody } from "../../atoms/Panels";
import { ButtonPrimary } from "../../atoms/Buttons/Buttons";
import LabelAndTextbox from "../../molecules/InfoWithLabels/LabelAndTextbox";
import passwordUpdateModel from "../../../models/passwordUpdateModel";
import { checkValidity } from "../../../utils/Validity";
import { PanelFooter } from "../../atoms/Panels";
import { QuaternaryDarkHeading } from "../../atoms/Heading";

const MainPanel = styled(PanelBody)`
  border-radius: ${(p) => p.theme.borderRadius.normal};
  box-shadow: ${(p) => p.theme.shadow.BoxShadow};
  margin-bottom: ${(p) => p.theme.size.normal};
  background-color: ${(p) => p.theme.color.whiteBg};
`;

const FormGroup = styled.div`
  margin-bottom: ${(p) => p.theme.size.exTiny};
  border-bottom: 1px solid ${(p) => p.theme.color.secondaryDivide};
`;

const Heading = styled(QuaternaryDarkHeading)`
  margin-bottom: ${(p) => p.theme.size.distance};
  margin-left: ${(p) => p.theme.size.exTiny};
`;

const FormFooter = styled(PanelFooter)`
  padding-left: 0;
  padding-right: 0;
`;

export default (props) => {
  let formData = passwordUpdateModel;
  const [, updateState] = useState();
  const forceUpdate = useCallback(() => updateState({}), []);

  const onTextboxChange = (e) => {
    formData = formData || {};
    const { name, value } = e.target;

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

  const onUpdate = (e) => {
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
      const profileData = {};
      for (const formIdentifier in formData) {
        profileData[formIdentifier] = formData[formIdentifier].value;
      }

      props.onUpdate(profileData);
    }
  };

  const { currentPassword, newPassword, confirmPassword } = formData;
  const isFormValid = checkIsFormValid();

  return (
    <Fragment>
      <Heading>Change Password</Heading>
      <MainPanel>
        <form onSubmit={(e) => onUpdate(e)} method="POST">
          <Fragment>
            <FormGroup>
              <LabelAndTextbox
                label="Current password"
                name="currentPassword"
                type="password"
                placeholder="Enter current password"
                value={currentPassword.value}
                onChange={onTextboxChange}
              />
            </FormGroup>
            <FormGroup>
              <LabelAndTextbox
                label="New password"
                name="newPassword"
                placeholder="Enter new password"
                value={newPassword.value}
                type="password"
                onChange={onTextboxChange}
              />
            </FormGroup>
            <FormGroup>
              <LabelAndTextbox
                label="Confirm new password"
                name="confirmPassword"
                placeholder="Confirm new password"
                value={confirmPassword.value}
                type="password"
                onChange={onTextboxChange}
              />
            </FormGroup>
          </Fragment>
          <FormFooter>
            <ButtonPrimary
              type="submit"
              size="xs"
              disabled={!props.isFormEnabled || !isFormValid}
            >
              <FontAwesomeIcon icon="pencil-alt" className="me-1" />
              Change Password
            </ButtonPrimary>
          </FormFooter>
        </form>
      </MainPanel>
    </Fragment>
  );
};
